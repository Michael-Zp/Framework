using System.Windows;
using System.Windows.Controls;
using Framework;
using System.IO;

namespace LevelEditor
{
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private Level levelData = new Level();

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			TraverseLogicalTree(canvas);
			levelData.ObjIntoBinFile(@"..\..\level.data");
			Close();
		}

		private void TraverseLogicalTree(DependencyObject dependencyObject)
		{
			if (ReferenceEquals(null, dependencyObject)) return;
			var childern = LogicalTreeHelper.GetChildren(dependencyObject);
			foreach (var child in childern)
			{
				var type = child.GetType();
				if (typeof(Image) == type)
				{
					var image = child as Image;
					Convert(image);
				}
				var logicalChild = child as DependencyObject;
				TraverseLogicalTree(logicalChild);
			}
		}

		/// <summary>
		/// Creates a sprite by transforming canvas coordinate system into [0,1]² with y-axis heading upwards
		/// extracting texture source and image name
		/// </summary>
		/// <param name="image">Input Image UIElement to convert</param>
		private void Convert(Image image)
		{
			var leftTop = (Vector)image.TranslatePoint(new Point(0, 0), canvas);
			var rightBottom = leftTop + (Vector)image.RenderSize;
			//normalize
			leftTop /= canvas.ActualWidth;
			rightBottom /= canvas.ActualHeight;
			//flip y coordinate
			var sprite = new Sprite((float)leftTop.X, (float)rightBottom.X, (float)(1 - rightBottom.Y), (float)(1 - leftTop.Y));
			var Layer = Canvas.GetZIndex(image);
			sprite.Name = image.Name;
			var source = image.Source?.ToString();
			if (!ReferenceEquals(null, source))
			{
				//sprite.TextureName = source.Substring(source.LastIndexOf(',') + 1);
				sprite.TextureName = Path.GetFileName(source);
			}
			levelData.Add(Layer, sprite);
		}

		private double Convert(double value)
		{
			if (double.IsNaN(value)) return 0; //throw new ArgumentException("Do not use automatic values!");
			return value;
		}
	}
}
