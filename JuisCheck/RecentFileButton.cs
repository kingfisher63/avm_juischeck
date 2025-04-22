using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace JuisCheck
{
	public class RecentFileButton : Button
	{
		static RecentFileButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(RecentFileButton), new FrameworkPropertyMetadata(typeof(RecentFileButton)));
		}

		// DependencyProperty: DirPath

		public static readonly DependencyProperty DirPathProperty = DependencyProperty.Register("DirPath", typeof(string), typeof(RecentFileButton));

		public string DirPath
		{
			get => (string)GetValue(DirPathProperty);
			set =>         SetValue(DirPathProperty, value);
		}

		// DependencyProperty: DirPathFontSize

		public static readonly DependencyProperty DirPathFontSizeProperty = DependencyProperty.Register("DirPathFontSize", typeof(double), typeof(RecentFileButton));

		public double DirPathFontSize
		{
			get => (double)GetValue(DirPathFontSizeProperty);
			set =>         SetValue(DirPathFontSizeProperty, value);
		}

		// DependencyProperty: DirPathFontWeight

		public static readonly DependencyProperty DirPathFontWeightProperty = DependencyProperty.Register("DirPathFontWeight", typeof(FontWeight), typeof(RecentFileButton));

		public FontWeight DirPathFontWeight
		{
			get => (FontWeight)GetValue(DirPathFontWeightProperty);
			set =>             SetValue(DirPathFontWeightProperty, value);
		}

		// DependencyProperty: DirPathForeground

		public static readonly DependencyProperty DirPathForegroundProperty = DependencyProperty.Register("DirPathForeground", typeof(Brush), typeof(RecentFileButton));

		public Brush DirPathForeground
		{
			get => (Brush)GetValue(DirPathForegroundProperty);
			set =>        SetValue(DirPathForegroundProperty, value);
		}

		// DependencyProperty: DirPathMargin

		public static readonly DependencyProperty DirPathMarginProperty = DependencyProperty.Register("DirPathMargin", typeof(Thickness), typeof(RecentFileButton));

		public Thickness DirPathMargin
		{
			get => (Thickness)GetValue(DirPathMarginProperty);
			set =>            SetValue(DirPathMarginProperty, value);
		}

		// DependencyProperty: FileName

		public static readonly DependencyProperty FileNameProperty = DependencyProperty.Register("FileName", typeof(string), typeof(RecentFileButton));

		public string FileName
		{
			get => (string)GetValue(FileNameProperty);
			set =>         SetValue(FileNameProperty, value);
		}

		// DependencyProperty: FileNameFontSize

		public static readonly DependencyProperty FileNameFontSizeProperty = DependencyProperty.Register("FileNameFontSize", typeof(double), typeof(RecentFileButton));

		public double FileNameFontSize
		{
			get => (double)GetValue(FileNameFontSizeProperty);
			set =>         SetValue(FileNameFontSizeProperty, value);
		}

		// DependencyProperty: FileNameFontWeight

		public static readonly DependencyProperty FileNameFontWeightProperty = DependencyProperty.Register("FileNameFontWeight", typeof(FontWeight), typeof(RecentFileButton));

		public FontWeight FileNameFontWeight
		{
			get => (FontWeight)GetValue(FileNameFontWeightProperty);
			set =>             SetValue(FileNameFontWeightProperty, value);
		}

		// DependencyProperty: FileNameForeground

		public static readonly DependencyProperty FileNameForegroundProperty = DependencyProperty.Register("FileNameForeground", typeof(Brush), typeof(RecentFileButton));

		public Brush FileNameForeground
		{
			get => (Brush)GetValue(FileNameForegroundProperty);
			set =>        SetValue(FileNameForegroundProperty, value);
		}

		// DependencyProperty: FileNameMargin

		public static readonly DependencyProperty FileNameMarginProperty = DependencyProperty.Register("FileNameMargin", typeof(Thickness), typeof(RecentFileButton));

		public Thickness FileNameMargin
		{
			get => (Thickness)GetValue(FileNameMarginProperty);
			set =>            SetValue(FileNameMarginProperty, value);
		}

		// DependencyProperty: Image

		public static readonly DependencyProperty ImageProperty =  DependencyProperty.Register("Image", typeof(ImageSource), typeof(RecentFileButton));

		public ImageSource Image
		{
			get => (ImageSource)GetValue(ImageProperty);
			set =>              SetValue(ImageProperty, value);
		}

		// DependencyProperty: ImageHeight

		public static readonly DependencyProperty ImageHeightProperty = DependencyProperty.Register("ImageHeight", typeof(double), typeof(RecentFileButton));

		public double ImageHeight
		{
			get => (double)GetValue(ImageHeightProperty);
			set =>         SetValue(ImageHeightProperty, value);
		}

		// DependencyProperty: ImageMargin

		public static readonly DependencyProperty ImageMarginProperty = DependencyProperty.Register("ImageMargin", typeof(Thickness), typeof(RecentFileButton));

		public Thickness ImageMargin
		{
			get => (Thickness)GetValue(ImageMarginProperty);
			set =>            SetValue(ImageMarginProperty, value);
		}

		// DependencyProperty: ImageWidth

        public static readonly DependencyProperty ImageWidthProperty = DependencyProperty.Register("ImageWidth", typeof(double), typeof(RecentFileButton));

		public double ImageWidth
		{
			get => (double)GetValue(ImageWidthProperty);
			set =>         SetValue(ImageWidthProperty, value);
        }

		// DependencyProperty: MouseOverBackground

		public static readonly DependencyProperty MouseOverBackgroundProperty = DependencyProperty.Register("MouseOverBackground", typeof(Brush), typeof(RecentFileButton));

		public Brush MouseOverBackground
		{
			get => (Brush)GetValue(MouseOverBackgroundProperty);
			set =>        SetValue(MouseOverBackgroundProperty, value);
		}

		// DependencyProperty: MouseOverBorderBrush

		public static readonly DependencyProperty MouseOverBorderBrushProperty = DependencyProperty.Register("MouseOverBorderBrush", typeof(Brush), typeof(RecentFileButton));

		public Brush MouseOverBorderBrush
		{
			get => (Brush)GetValue(MouseOverBorderBrushProperty);
			set =>        SetValue(MouseOverBorderBrushProperty, value);
		}
	}
}
