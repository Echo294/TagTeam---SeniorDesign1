
from flask import Flask, request, jsonify
from werkzeug.security import generate_password_hash
import sqlite3
import os
from flask import send_from_directory

app = Flask(__name__)

DATABASE = os.path.join(
    os.path.dirname(__file__), "tagteam.db"
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

    if (not username or not email or
            not isinstance(password, str) or
            len(password) < 8):
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

@app.get("/api/health")
def health():
    return jsonify({"status": "online"})

WEBSITE = os.path.abspath(
    os.path.join(os.path.dirname(__file__), "..", "Tag Team Website")
)

@app.get("/")
def homepage():
    return send_from_directory(WEBSITE, "index.html")

@app.get("/<path:filename>")
def website_files(filename):
    return send_from_directory(WEBSITE, filename)

if __name__ == "__main__":
    init_db()
    app.run(host="127.0.0.1", port=5000, debug=True)