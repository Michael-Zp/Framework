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
			TraverseLogicalTree(canvas, string.Empty);
			var dir = System.IO.Path.GetDirectoryName(PathTools.GetSourceFilePath()) + "/../LevelConsumer/Resources/";
			levelData.ObjIntoBinFile(dir + "level.data");
			string[] args = Environment.GetCommandLineArgs();
			if(args.Length > 1)
			{
				if("close" == args[1].ToLower()) Close();
			}
		}

		private void TraverseLogicalTree(DependencyObject dependencyObject, string parentName)
		{
			if (ReferenceEquals(null, dependencyObject)) return;
			var childern = LogicalTreeHelper.GetChildren(dependencyObject);
			foreach (var child in childern)
			{
				var type = child.GetType();
				if (typeof(Image) == type)
				{
					var image = child as Image;
					Convert(image, canvas, parentName);
				}
				else if (typeof(Ellipse) == type)
				{
					var collider = child as Ellipse;
					Convert(collider, parentName);
				}
				var logicalChild = child as FrameworkElement;
				TraverseLogicalTree(logicalChild, ResolveName(logicalChild.Name, parentName));
			}
		}

		private void Convert(Ellipse collider, string parentName)
		{
			var bounds = ConvertBounds(collider, canvas);
			var circle = CircleExtensions.CreateFromBox(bounds);
			levelData.Add(new ColliderCircle(ResolveName(collider.Name, parentName), circle));
		}

		private string ResolveName(string name, string parentName)
		{
			return string.IsNullOrWhiteSpace(name) ? parentName : name;
		}

		/// <summary>
		/// Creates a sprite by transforming canvas coordinate system into [0,1]² with y-axis heading upwards
		/// extracting texture source and image name
		/// </summary>
		/// <param name="image">Input Image UIElement to convert</param>
		private void Convert(Image image, Canvas canvas, string parentName)
		{
			var bounds = ConvertBounds(image, canvas);
			var layer = Canvas.GetZIndex(image);
			var sprite = new Sprite(ResolveName(image.Name, parentName), bounds, layer);
			var source = image.Source?.ToString();
			if (!ReferenceEquals(null, source))
			{
				var fileName = System.IO.Path.GetFileName(source);
				//sprite.TextureName = source.Substring(source.LastIndexOf(',') + 1);
				sprite.TextureName = source;
				//todo1: register bitmap list, convert imagesource into bitmap directly
				sprite.Texture = new System.Drawing.Bitmap(@"..\..\" + fileName);
			}
			levelData.Sprites.Add(sprite);
		}

		private static Box2D ConvertBounds(UIElement element, Canvas canvas)
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
