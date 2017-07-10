1. Execute program
1. Give each vertex a different color
1. Execute and compare
	+ Note that OpenGL is a state machine. Color stays until other color is selected.
1. Resize the window. What happens? 
1. Add `wnd.Resize += (s, a) => GL.Viewport(0, 0, wnd.Width, wnd.Height);` after creating the window. What happens when you resize the window now?
1. Make the whole quad move to the right over time
1. Move the render code for the quad into a function `void DrawBox(Box2D box)`
1. Make the whole quad move to some direction over time
1. Make the quad change directions if it leaves the window
1. Render a number of boxes that move into different directions
	+ These will our enemies
1. Add a box that will be our player
	1. This box moves to the left if `Keyboard.GetState().IsKeyDown(Key.Left)` is true
	1. This box moves to the right if `Keyboard.GetState().IsKeyDown(Key.Right)` is true