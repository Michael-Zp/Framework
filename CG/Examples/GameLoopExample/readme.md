1. Execute program
2. Switch off Vsync (uncomment `wnd.VSync...`). What happens to the animations?
3. Give each vertex a different color
4. Execute and compare
	+ Note that OpenGL is a state machine. Color stays until other color is selected.
5. Resize the window. What happens? 
6. Add `wnd.Resize += (s, a) => GL.Viewport(0, 0, wnd.Width, wnd.Height);` after creating the window. What happens when you resize the window now?
7. Move the render code for the quad into a function `void DrawBox(Box2D box)`
8. Make the whole quad move to the right over time
9. Make the whole quad move to some direction over time
10. Make the quad change directions if it leaves the window
11. Render a number of boxes that move into different directions
	+ These will be our enemies
12. Add a box that will be our player
	1. This box moves to the left if `Keyboard.GetState().IsKeyDown(Key.Left)` is true
	2. This box moves to the right if `Keyboard.GetState().IsKeyDown(Key.Right)` is true