using System;
using System.Windows.Forms;
using OpenTK;

namespace ShaderForm
{
	public class FlyCamera
	{
		public Vector3 Position = new Vector3(0);
		public Vector3 Rotation = new Vector3(0);

		public bool IsActive { get { return fwd || left || right || back || up || down; } }

		public void Update(float mouseX, float mouseY, bool leftPressed)
		{
			float mouseXDelta = lastMouseX - mouseX;
			lastMouseX = mouseX;
			float mouseYDelta = lastMouseY - mouseY;
			lastMouseY = mouseY;

			Vector3 camLeft = new Vector3(-1, 0, 0);
			Vector3 camFwdTmp = new Vector3(0, 0, 1);

			/** X-Rotation **/
			float xRotation2 = -(float)(mouseYDelta * Math.PI / 180f);
			if (leftPressed) Rotation.X += xRotation2;
			Rotation.X = (float)(Rotation.X % (2.0 * Math.PI));
			camFwdTmp = RotateX(camFwdTmp, -Rotation.X);
			camLeft = RotateX(camLeft, -Rotation.X);

			/** Y-Rotation **/
			float yRotation2 = -(float)(mouseXDelta * Math.PI / 180f);
			if (leftPressed) Rotation.Y += yRotation2;
			Rotation.Y = (float)(Rotation.Y % (2.0 * Math.PI));
			camFwdTmp = RotateY(camFwdTmp, Rotation.Y);
			camLeft = RotateY(camLeft, Rotation.Y);

			var camUpTemp = -Vector3.Cross(camFwdTmp, camLeft);

			float speed = FlyCamera.speed + ((fast) ? FlyCamera.speed * 2 : 0);

			if (fwd) Position += camFwdTmp * speed;
			if (back) Position -= camFwdTmp * speed;
			if (left) Position += camLeft * speed;
			if (right) Position -= camLeft * speed;
			if (up) Position += camUpTemp * speed;
			if (down) Position -= camUpTemp * speed;

			camFwd = camFwdTmp;
		}

		public void KeyChange(Keys key, bool pressed)
		{
			switch (key)
			{
				case Keys.W: fwd = pressed; break;
				case Keys.A: left = pressed; break;
				case Keys.S: back = pressed; break;
				case Keys.D: right = pressed; break;
				case Keys.Q: up = pressed; break;
				case Keys.E: down = pressed; break;
				case Keys.ShiftKey: fast = pressed; break;
			}
		}

		private const float speed = 0.05f; //todo1: Has to be adapted for every shader.. scene size matters..
		private Vector3 camFwd = new Vector3(0, 0, 1);
		private bool fwd, back, left, right, up, down, fast = false;
		private float lastMouseX = 0;
		private float lastMouseY = 0;

		private Vector3 RotateX(Vector3 vec, float angle)
		{

			Matrix4 rotateXM = Matrix4.CreateRotationX(angle);
			return Vector3.Transform(vec, rotateXM);
		}

		private Vector3 RotateY(Vector3 vec, float angle)
		{
			Matrix4 rotateZM = Matrix4.CreateRotationY(angle);
			return Vector3.Transform(vec, rotateZM);
		}
	}
}
