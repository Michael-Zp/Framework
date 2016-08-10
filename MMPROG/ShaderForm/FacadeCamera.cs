using System;
using System.Windows.Forms;

namespace ShaderForm
{
	public class FacadeCamera
	{
		public void AddKeyFrames(float time, IUniforms uniforms)
		{
			for (int i = 0; i < 3; ++i)
			{
				uniforms.Add(posUniformNames[i]);
				var kfsPos = uniforms.GetKeyFrames(posUniformNames[i]);
				kfsPos.AddUpdate(time, camera.Position[i]);

				uniforms.Add(rotUniformNames[i]);
				var kfsRot = uniforms.GetKeyFrames(rotUniformNames[i]);
				kfsRot.AddUpdate(time, camera.Rotation[i]);
			}
		}

		public void KeyChange(Keys keyCode, bool pressed)
		{
			camera.KeyChange(keyCode, pressed);
			if (IsActive)
			{
				var focused = FormCamera.ActiveForm;
				formCamera.Visible = true;
				formCamera.Set(camera);
				focused.Activate();
			}
		}

		public bool IsActive { get { return camera.IsActive; } }

		public void Reset()
		{
			camera = new FlyCamera();
		}

		public void Update(float mouseX, float mouseY, bool mouseDown)
		{
			camera.Update(mouseX, mouseY, mouseDown);
		}

		public void UpdateFromUniforms(IUniforms uniforms, float time)
		{
			for (int i = 0; i < 3; ++i)
			{
				var kfsPos = uniforms.GetKeyFrames(posUniformNames[i]);
				if (null == kfsPos) return;
				var value = kfsPos.Interpolate(time);
				camera.Position[i] = value;

				var kfsRot = uniforms.GetKeyFrames(rotUniformNames[i]);
				if (null == kfsRot) return;
				var valueRot = kfsRot.Interpolate(time);
				camera.Rotation[i] = valueRot;
			}
		}

		public void SetUniforms(ISetUniform visualContext)
		{
			for (int i = 0; i < 3; ++i)
			{
				visualContext.SetUniform(posUniformNames[i], camera.Position[i]);
				visualContext.SetUniform(rotUniformNames[i], camera.Rotation[i]);
			}
		}

		private FlyCamera camera = new FlyCamera();
		private FormCamera formCamera = new FormCamera();
		private string[] posUniformNames = { "iCamPosX", "iCamPosY", "iCamPosZ" };
		private string[] rotUniformNames = { "iCamRotX", "iCamRotY", "iCamRotZ" };
	}
}
