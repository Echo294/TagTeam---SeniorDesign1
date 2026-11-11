
"use strict";

document.addEventListener("DOMContentLoaded", async () => {
    const balance = document.getElementById("diamondBalance");
    const message = document.getElementById("shopMessage");

    try {
        const response = await fetch("/api/me");

        if (response.status === 401) {
            window.location.href = "login.html";
            return;
        }

        if (!response.ok) {
            throw new Error("Unable to load account.");
        }

        const player = await response.json();

        balance.textContent =
            Number(player.premium_currency).toLocaleString();

    } catch (error) {
        console.error(error);
        balance.textContent = "Unavailable";
        message.textContent =
            "Could not load your diamond balance.";
    }
});
