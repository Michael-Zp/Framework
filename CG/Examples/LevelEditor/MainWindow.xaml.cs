using System.Windows;
using System.Windows.Controls;
using System;
using System.Windows.Shapes;
using DMS.System;
using DMS.Geometry;

namespace LevelData
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
			//do not use autosize for canvas -> set a fixed size
			levelData.Bounds.SizeX = (float)canvas.ActualWidth;
			levelData.Bounds.SizeY = (float)canvas.ActualHeight;
			TraverseLogicalTree(canvas);
			var dir = System.IO.Path.GetDirectoryName(PathTools.GetSourceFilePath()) + "/../LevelConsumer/Resources/";
			levelData.ObjIntoBinFile(dir + "level.data");
			string[] args = Environment.GetCommandLineArgs();
			if(args.Length > 1)
			{
				if("close" == args[1].ToLower()) Close();
			}
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
				else if (typeof(Ellipse) == type)
				{
					var collider = child as Ellipse;
					Convert(collider);
				}
				var logicalChild = child as DependencyObject;
				TraverseLogicalTree(logicalChild);
			}
		}

		private void Convert(Ellipse collider)
		{
			var bounds = ConvertBounds(collider);
			var circle = CircleExtensions.CreateFromBox(bounds);
			levelData.Add(new ColliderCircle(circle));
		}

		/// <summary>
		/// Creates a sprite by transforming canvas coordinate system into [0,1]² with y-axis heading upwards
		/// extracting texture source and image name
		/// </summary>
		/// <param name="image">Input Image UIElement to convert</param>
		private void Convert(Image image)
		{
			var bounds = ConvertBounds(image);
			var sprite = new Sprite(bounds);
			var Layer = Canvas.GetZIndex(image);
			sprite.Name = image.Name;
			var source = image.Source?.ToString();
			if (!ReferenceEquals(null, source))
			{
				//sprite.TextureName = source.Substring(source.LastIndexOf(',') + 1);
				sprite.TextureName = System.IO.Path.GetFileName(source);
				//todo1: register bitmap list
				sprite.Texture = new System.Drawing.Bitmap(@"..\..\" + sprite.TextureName);
			}
			levelData.Add(Layer, sprite);
		}

		private Box2D ConvertBounds(UIElement element)
		{
			var p00 = new Point(0, 0);
			var p11 = p00 + (Vector)element.RenderSize;
			var leftTop = (Vector)element.TranslatePoint(p00, canvas);
			var rightBottom = (Vector)element.TranslatePoint(p11, canvas);
			//flip y coordinate
			return Box2dExtensions.CreateFromMinMax(
				(float)leftTop.X, (float)(canvas.ActualHeight - rightBottom.Y),
				(float)rightBottom.X, (float)(canvas.ActualHeight - leftTop.Y));
		}
	}
}
