import os
import sqlite3

from flask import (
    Flask,
    request,
    jsonify,
    session,
    send_from_directory
)

from werkzeug.security import (
    generate_password_hash,
    check_password_hash
)

app = Flask(__name__)

app.secret_key = os.environ["FLASK_SECRET_KEY"]

app.config.update(
    SESSION_COOKIE_HTTPONLY=True,
    SESSION_COOKIE_SAMESITE="Lax"
)

DATABASE = os.path.join(
    os.path.dirname(__file__), "tagteam.db"
)

WEBSITE = os.path.abspath(
    os.path.join(
        os.path.dirname(__file__),
        "..",
        "Tag Team Website"
    )
)



def get_db():
    connection = sqlite3.connect(DATABASE)
    connection.row_factory = sqlite3.Row
    return connection


def init_db():
    with get_db() as db:
        db.execute("""
            CREATE TABLE IF NOT EXISTS users (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                username TEXT NOT NULL UNIQUE,
                email TEXT NOT NULL UNIQUE,
                password_hash TEXT NOT NULL,
                gold INTEGER NOT NULL DEFAULT 0,
                premium_currency INTEGER NOT NULL DEFAULT 0
            )
        """)



@app.post("/api/register")
def register():
    data = request.get_json(silent=True) or {}

    username = str(data.get("username", "")).strip()
    email = str(data.get("email", "")).strip().lower()
    password = data.get("password", "")

    if (
        not username
        or not email
        or not isinstance(password, str)
        or len(password) < 8
    ):
        return jsonify({
            "error": "Enter a username, email, and password of at least 8 characters."
        }), 400

    password_hash = generate_password_hash(password)

    try:
        with get_db() as db:
            db.execute("""
                INSERT INTO users
                (username, email, password_hash)
                VALUES (?, ?, ?)
            """, (username, email, password_hash))

    except sqlite3.IntegrityError:
        return jsonify({
            "error": "Username or email already exists."
        }), 409

    return jsonify({
        "message": "Account created successfully!"
    }), 201



@app.post("/api/login")
def login():
    data = request.get_json(silent=True) or {}

    email = str(data.get("email", "")).strip().lower()
    password = data.get("password", "")

    if not email or not isinstance(password, str):
        return jsonify({
            "error": "Invalid email or password."
        }), 400

    with get_db() as db:
        user = db.execute("""
            SELECT * FROM users WHERE email = ?
        """, (email,)).fetchone()

    if user is None or not check_password_hash(
        user["password_hash"], password
    ):
        return jsonify({
            "error": "Invalid email or password."
        }), 401

    session.clear()
    session["user_id"] = user["id"]

    return jsonify({
        "message": "Login successful!",
        "username": user["username"]
    }), 200



@app.get("/api/me")
def current_player():
    user_id = session.get("user_id")

    if user_id is None:
        return jsonify({
            "error": "Not logged in."
        }), 401

    with get_db() as db:
        user = db.execute("""
            SELECT id, username, email,
                   gold, premium_currency
            FROM users
            WHERE id = ?
        """, (user_id,)).fetchone()

    if user is None:
        session.clear()
        return jsonify({
            "error": "Account not found."
        }), 401

    return jsonify(dict(user)), 200



@app.post("/api/logout")
def logout():
    session.clear()

    return jsonify({
        "message": "Logged out successfully!"
    }), 200



@app.get("/api/health")
def health():
    return jsonify({"status": "online"})


@app.get("/")
def homepage():
    return send_from_directory(WEBSITE, "index.html")


@app.get("/<path:filename>")
def website_files(filename):
    return send_from_directory(WEBSITE, filename)



if __name__ == "__main__":
    init_db()
    app.run(
        host="127.0.0.1",
        port=5000,
        debug=True
    )
