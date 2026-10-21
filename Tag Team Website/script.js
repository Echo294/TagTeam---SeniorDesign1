"use strict";


document.addEventListener("DOMContentLoaded", () => {

    console.log("TagTeam Player Portal loaded!");

    const registerForm = document.getElementById("registerForm");

    if (registerForm) {
        registerForm.addEventListener("submit", async (event) => {
            event.preventDefault();

            const message = document.getElementById("message");
            const submitButton = registerForm.querySelector(
                'button[type="submit"]'
            );

            const username = document.getElementById("username").value.trim();
            const email = document.getElementById("email").value.trim();
            const password = document.getElementById("password").value;

            if (!message) {
                console.error("Registration message element is missing.");
                return;
            }

            // Basic validation
            if (!username || !email || password.length < 8) {
                showMessage(
                    message,
                    "Please complete all fields. Password must be at least 8 characters.",
                    "error"
                );
                return;
            }

            submitButton.disabled = true;
            showMessage(message, "Creating your account...", "info");

            try {
                const response = await fetch("/api/register", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify({
                        username: username,
                        email: email,
                        password: password
                    })
                });

                const result = await response.json();

                if (response.ok) {
                    showMessage(
                        message,
                        result.message || "Account created successfully!",
                        "success"
                    );

                    registerForm.reset();

                } else {
                    showMessage(
                        message,
                        result.error || "Registration failed. Please try again.",
                        "error"
                    );
                }

            } catch (error) {
                console.error("Registration error:", error);

                showMessage(
                    message,
                    "Unable to connect to the server. Please try again.",
                    "error"
                );

            } finally {
                submitButton.disabled = false;
            }
        });
    }

    function showMessage(element, text, type) {
        element.textContent = text;

        switch (type) {
            case "success":
                element.style.color = "#4ade80";
                break;

            case "error":
                element.style.color = "#f87171";
                break;

            default:
                element.style.color = "#ffffff";
        }
    }

});



document.addEventListener("DOMContentLoaded", () => {

    const loginForm = document.getElementById("loginForm");

    if (!loginForm) return;

    loginForm.addEventListener("submit", async (event) => {
        event.preventDefault();

        const message = document.getElementById("message");
        const button = loginForm.querySelector('button[type="submit"]');

        const email = document.getElementById("email").value.trim();
        const password = document.getElementById("password").value;

        button.disabled = true;
        message.textContent = "Logging in...";
        message.style.color = "white";

        try {
            const response = await fetch("/api/login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ email, password })
            });

            const result = await response.json();

            if (response.ok) {
                message.textContent =
                    `Welcome back, ${result.username}!`;

                message.style.color = "#4ade80";

                loginForm.reset();

            } else {
                message.textContent =
                    result.error || "Login failed.";

                message.style.color = "#f87171";
            }

        } catch (error) {
            console.error("Login error:", error);

            message.textContent =
                "Could not connect to the server.";

            message.style.color = "#f87171";

        } finally {
            button.disabled = false;
        }
    });
});