import tkinter as tk
import random
import math

# ==================================
# WINDOW
# ==================================
WIDTH = 600
HEIGHT = 800

root = tk.Tk()
root.title("RETRO ARCADE BREAKER")

canvas = tk.Canvas(
    root,
    width=WIDTH,
    height=HEIGHT,
    bg="black",
    highlightthickness=0
)
canvas.pack()

# ==================================
# GAME VARIABLES
# ==================================
score = 0
lives = 3
level = 1

game_running = True

BALL_SPEED = 7
dx = 0
dy = 0

ball_attached = True

# Paddle
PADDLE_WIDTH = 120
PADDLE_SPEED = 9

move_left_pressed = False
move_right_pressed = False

restart_button = None

# ==================================
# BORDER
# ==================================
canvas.create_rectangle(
    10, 10,
    WIDTH - 10,
    HEIGHT - 10,
    outline="white",
    width=4
)

# ==================================
# UI
# ==================================
score_text = canvas.create_text(
    100, 40,
    text="SCORE: 0000",
    fill="white",
    font=("Consolas", 20, "bold")
)

lives_text = canvas.create_text(
    300, 40,
    text="LIVES: 3",
    fill="white",
    font=("Consolas", 20, "bold")
)

level_text = canvas.create_text(
    500, 40,
    text="LEVEL: 1",
    fill="white",
    font=("Consolas", 20, "bold")
)

message_text = canvas.create_text(
    WIDTH//2,
    HEIGHT//2,
    text="MOVE PADDLE TO START",
    fill="white",
    font=("Consolas", 24, "bold")
)

# ==================================
# BRICKS
# ==================================
brick_colors = [
    "#ff0000",
    "#ff6600",
    "#ffaa00",
    "#ffee00",
    "#55cc00",
    "#00ccff"
]

bricks = []

def create_bricks():

    global bricks

    for b in bricks:
        canvas.delete(b)

    bricks.clear()

    rows = 6
    cols = 12

    brick_width = 44
    brick_height = 20

    start_x = 35
    start_y = 100

    for row in range(rows):
        for col in range(cols):

            x1 = start_x + col * brick_width
            y1 = start_y + row * brick_height

            x2 = x1 + brick_width - 3
            y2 = y1 + brick_height - 3

            brick = canvas.create_rectangle(
                x1, y1, x2, y2,
                fill=brick_colors[row % len(brick_colors)],
                outline="black"
            )

            bricks.append(brick)

create_bricks()

# ==================================
# PADDLE
# ==================================
paddle = canvas.create_rectangle(
    WIDTH//2 - PADDLE_WIDTH//2,
    720,
    WIDTH//2 + PADDLE_WIDTH//2,
    735,
    fill="#00aaff",
    outline=""
)

# ==================================
# BALL
# ==================================
BALL_SIZE = 18

ball = canvas.create_oval(
    WIDTH//2 - BALL_SIZE,
    690,
    WIDTH//2 + BALL_SIZE,
    690 + BALL_SIZE*2,
    fill="white",
    outline=""
)

# ==================================
# FUNCTIONS
# ==================================
def update_ui():

    canvas.itemconfig(
        score_text,
        text=f"SCORE: {score:04}"
    )

    canvas.itemconfig(
        lives_text,
        text=f"LIVES: {lives}"
    )

    canvas.itemconfig(
        level_text,
        text=f"LEVEL: {level}"
    )

def attach_ball():

    global ball_attached
    global dx
    global dy

    ball_attached = True

    dx = 0
    dy = 0

    paddle_pos = canvas.coords(paddle)

    paddle_center = (
        paddle_pos[0] + paddle_pos[2]
    ) / 2

    canvas.coords(
        ball,
        paddle_center - BALL_SIZE,
        690,
        paddle_center + BALL_SIZE,
        690 + BALL_SIZE * 2
    )

    canvas.itemconfig(
        message_text,
        text="MOVE PADDLE TO START",
        fill="white"
    )

def launch_ball():

    global ball_attached
    global dx
    global dy

    if ball_attached:

        ball_attached = False

        dx = random.choice([-5, 5])
        dy = -5

        canvas.itemconfig(
            message_text,
            text=""
        )

def restart_game():

    global score
    global lives
    global level
    global game_running
    global BALL_SPEED

    score = 0
    lives = 3
    level = 1

    BALL_SPEED = 7

    game_running = True

    update_ui()

    create_bricks()

    canvas.coords(
        paddle,
        WIDTH//2 - PADDLE_WIDTH//2,
        720,
        WIDTH//2 + PADDLE_WIDTH//2,
        735
    )

    attach_ball()

    restart_button.place_forget()

def game_over():

    global game_running

    game_running = False

    canvas.itemconfig(
        message_text,
        text="GAME OVER",
        fill="red"
    )

    restart_button.place(
        x=WIDTH//2 - 70,
        y=HEIGHT//2 + 50
    )

def check_collision(a, b):

    return (
        a[2] >= b[0] and
        a[0] <= b[2] and
        a[3] >= b[1] and
        a[1] <= b[3]
    )

# ==================================
# KEY INPUT
# ==================================
def key_press(event):

    global move_left_pressed
    global move_right_pressed

    if not game_running:
        return

    if event.keysym == "Left":
        move_left_pressed = True
        launch_ball()

    elif event.keysym == "Right":
        move_right_pressed = True
        launch_ball()

def key_release(event):

    global move_left_pressed
    global move_right_pressed

    if event.keysym == "Left":
        move_left_pressed = False

    elif event.keysym == "Right":
        move_right_pressed = False

root.bind("<KeyPress>", key_press)
root.bind("<KeyRelease>", key_release)

# ==================================
# SMOOTH PADDLE
# ==================================
def update_paddle():

    if game_running:

        pos = canvas.coords(paddle)

        moved = False

        if move_left_pressed and pos[0] > 15:
            canvas.move(paddle, -PADDLE_SPEED, 0)
            moved = True

        if move_right_pressed and pos[2] < WIDTH - 15:
            canvas.move(paddle, PADDLE_SPEED, 0)
            moved = True

        if ball_attached and moved:

            paddle_pos = canvas.coords(paddle)

            paddle_center = (
                paddle_pos[0] + paddle_pos[2]
            ) / 2

            canvas.coords(
                ball,
                paddle_center - BALL_SIZE,
                690,
                paddle_center + BALL_SIZE,
                690 + BALL_SIZE * 2
            )

    root.after(10, update_paddle)

# ==================================
# BALL MOVEMENT
# ==================================
def move_ball():

    global dx, dy
    global score
    global lives
    global level
    global BALL_SPEED

    if game_running:

        if not ball_attached:
            canvas.move(ball, dx, dy)

        ball_pos = canvas.coords(ball)

        # WALLS
        if ball_pos[0] <= 12:
            dx = abs(dx)

        if ball_pos[2] >= WIDTH - 12:
            dx = -abs(dx)

        if ball_pos[1] <= 12:
            dy = abs(dy)

        # MISS BALL
        if ball_pos[3] >= HEIGHT:

            lives -= 1
            update_ui()

            if lives <= 0:
                game_over()
            else:
                attach_ball()

        # PADDLE COLLISION
        paddle_pos = canvas.coords(paddle)

        if check_collision(ball_pos, paddle_pos) and dy > 0:

            canvas.move(ball, 0, -10)

            paddle_center = (
                paddle_pos[0] + paddle_pos[2]
            ) / 2

            ball_center = (
                ball_pos[0] + ball_pos[2]
            ) / 2

            relative_hit = (
                (ball_center - paddle_center)
                / (PADDLE_WIDTH / 2)
            )

            bounce_angle = relative_hit * math.radians(60)

            dx = BALL_SPEED * math.sin(bounce_angle)
            dy = -BALL_SPEED * math.cos(bounce_angle)

        # BRICK COLLISION
        hit_brick = None

        for brick in bricks:

            brick_pos = canvas.coords(brick)

            if check_collision(ball_pos, brick_pos):

                hit_brick = brick

                overlap_left = ball_pos[2] - brick_pos[0]
                overlap_right = brick_pos[2] - ball_pos[0]
                overlap_top = ball_pos[3] - brick_pos[1]
                overlap_bottom = brick_pos[3] - ball_pos[1]

                min_overlap = min(
                    overlap_left,
                    overlap_right,
                    overlap_top,
                    overlap_bottom
                )

                if min_overlap == overlap_left:
                    dx = -abs(dx)

                elif min_overlap == overlap_right:
                    dx = abs(dx)

                elif min_overlap == overlap_top:
                    dy = -abs(dy)

                elif min_overlap == overlap_bottom:
                    dy = abs(dy)

                break

        if hit_brick:

            canvas.delete(hit_brick)
            bricks.remove(hit_brick)

            score += 10
            update_ui()

        # NEXT LEVEL
        if len(bricks) == 0:

            level += 1
            BALL_SPEED += 1.5

            angle = math.atan2(dy, dx)

            dx = BALL_SPEED * math.cos(angle)
            dy = BALL_SPEED * math.sin(angle)

            update_ui()
            create_bricks()
            attach_ball()

    root.after(16, move_ball)

# ==================================
# RESTART BUTTON
# ==================================
restart_button = tk.Button(
    root,
    text="RESTART",
    font=("Consolas", 18, "bold"),
    bg="black",
    fg="white",
    activebackground="red",
    activeforeground="white",
    command=restart_game
)

# ==================================
# START
# ==================================
update_ui()
attach_ball()
update_paddle()
move_ball()

root.mainloop()