using ControlClassLibrary;
using OpenTK;
using System.Windows.Forms;

namespace ShaderForm
{
	public partial class FormCamera : Form
	{
		public FormCamera()
		{
			InitializeComponent();
			this.LoadLayout();
		}

		public void Set(FlyCamera cam)
		{
			propertyGrid1.SelectedObject = new AdapterCam(cam);
		}

		public void SaveData()
		{
			this.SaveLayout();
		}
	}

	class AdapterCam
	{
		public float PositionX { get { return cam.Position.X; } set { cam.Position.X = value; } }
		public float PositionY { get { return cam.Position.Y; } set { cam.Position.Y = value; } }
		public float PositionZ { get { return cam.Position.Z; } set { cam.Position.Z = value; } }
		public float RotationX { get { return cam.Rotation.X; } set { cam.Rotation.X = value; } }
		public float RotationY { get { return cam.Rotation.Y; } set { cam.Rotation.Y = value; } }
		public float RotationZ { get { return cam.Rotation.Z; } set { cam.Rotation.Z = value; } }
		public AdapterCam(FlyCamera cam)
		{
			this.cam = cam;
		}

		private FlyCamera cam;
	}
}
