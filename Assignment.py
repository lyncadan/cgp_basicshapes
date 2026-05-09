import tkinter as tk

root = tk.Tk()
root.title("CGP Assignment - Night Scene")

canvas = tk.Canvas(root, width=500, height=400, bg="midnightblue")
canvas.pack()

# Moon
canvas.create_oval(380, 40, 450, 110, fill="lightyellow")

# Stars
for x, y in [(50, 50), (120, 80), (200, 40), (300, 100), (420, 150), (80, 200), (150, 250)]:
    canvas.create_oval(x, y, x+5, y+5, fill="white")

# Ground
canvas.create_rectangle(0, 300, 500, 400, fill="darkgreen")

# Tree trunk
canvas.create_rectangle(80, 220, 110, 300, fill="saddlebrown")

# Tree leaves
canvas.create_oval(50, 150, 140, 240, fill="forestgreen")

# Name
canvas.create_text(250, 370, text="Manilyn N. Caadan", font=("Arial", 18), fill="lightpink")

root.mainloop()
