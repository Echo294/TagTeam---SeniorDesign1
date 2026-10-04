const player1 = document.getElementById("player1");
const player2 = document.getElementById("player2");

const status = document.getElementById("status");
const resetButton = document.getElementById("reset-button");

let player1X = 100;
let player2X = 160;

let player1Y = 100;
let player2Y = 100;

let player1VelocityY = 0;
let player2VelocityY = 0;

let player1Grounded = true;
let player2Grounded = true;

const speed = 5;
const jumpPower = 12;
const gravity = 0.6;

const keys = {};


document.addEventListener("keydown", function(event) {

    keys[event.key] = true;

});


document.addEventListener("keyup", function(event) {

    keys[event.key] = false;

});


function updateGame() {

    // Player 1 movement

    if (keys["a"]) {
        player1X -= speed;
    }

    if (keys["d"]) {
        player1X += speed;
    }

    if (keys["w"] && player1Grounded) {
        player1VelocityY = jumpPower;
        player1Grounded = false;
    }


    // Player 2 movement

    if (keys["ArrowLeft"]) {
        player2X -= speed;
    }

    if (keys["ArrowRight"]) {
        player2X += speed;
    }

    if (keys["ArrowUp"] && player2Grounded) {
        player2VelocityY = jumpPower;
        player2Grounded = false;
    }


    // Gravity

    player1VelocityY -= gravity;
    player2VelocityY -= gravity;

    player1Y += player1VelocityY;
    player2Y += player2VelocityY;


    // Ground

    if (player1Y <= 100) {
        player1Y = 100;
        player1VelocityY = 0;
        player1Grounded = true;
    }

    if (player2Y <= 100) {
        player2Y = 100;
        player2VelocityY = 0;
        player2Grounded = true;
    }


    // Keep players inside game

    player1X = Math.max(0, Math.min(860, player1X));
    player2X = Math.max(0, Math.min(860, player2X));


    // Update positions

    player1.style.left = player1X + "px";
    player1.style.bottom = player1Y + "px";

    player2.style.left = player2X + "px";
    player2.style.bottom = player2Y + "px";


    checkGoal();

    requestAnimationFrame(updateGame);
}


function checkGoal() {

    const distance = Math.abs(player1X - player2X);

    if (player1X > 800 && player2X > 800) {

        status.textContent = "TAGTEAM COMPLETE!";

    } else if (distance > 300) {

        status.textContent = "Stay Together!";

    } else {

        status.textContent = "Work Together!";

    }

}


resetButton.addEventListener("click", function() {

    player1X = 100;
    player2X = 160;

    player1Y = 100;
    player2Y = 100;

    status.textContent = "Work Together!";

});


updateGame();