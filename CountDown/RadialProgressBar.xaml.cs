using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CountDown
{
  /// <summary>
  /// Interaction logic for RadialProgressBar.xaml
  /// </summary>
  public partial class RadialProgressBar : UserControl
  {
    public static readonly DependencyProperty IndicatorBrushProperty = DependencyProperty.Register("IndicatorBrush", typeof(Brush), typeof(RadialProgressBar));
    public Brush IndicatorBrush
    {
      get { return (Brush)this.GetValue(IndicatorBrushProperty); }
      set { this.SetValue(IndicatorBrushProperty, value); }
    }

    public static readonly DependencyProperty BackgroundBrushProperty = DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(RadialProgressBar));
    public Brush BackgroundBrush
    {
      get { return (Brush)this.GetValue(BackgroundBrushProperty); }
      set { this.SetValue(BackgroundBrushProperty, value); }
    }

    public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(RadialProgressBar));
    public Brush ProgressBorderBrush
    {
      get { return (Brush)this.GetValue(ProgressBorderBrushProperty); }
      set { this.SetValue(ProgressBorderBrushProperty, value); }
    }

    public static readonly DependencyProperty ProgressBorderBrushProperty = DependencyProperty.Register("ProgressBorderBrush", typeof(Brush), typeof(RadialProgressBar));
    public Brush Stroke
    {
      get { return (Brush)this.GetValue(StrokeProperty); }
      set { this.SetValue(StrokeProperty, value); }
    }

    public static readonly DependencyProperty StartAngleProperty = DependencyProperty.Register("StartAngle", typeof(double), typeof(RadialProgressBar));
    public double StartAngle
    {
      get { return (double)this.GetValue(StartAngleProperty); }
      set { this.SetValue(StartAngleProperty, value); }
    }

    public static readonly DependencyProperty EndAngleProperty = DependencyProperty.Register("EndAngle", typeof(double), typeof(RadialProgressBar));
    public double EndAngle
    {
      get { return (double)this.GetValue(EndAngleProperty); }
      set { this.SetValue(EndAngleProperty, value); }
    }

    public RadialProgressBar()
    {
      InitializeComponent();
    }
  }

  public class PercentageConverter : MarkupExtension, IValueConverter
  {
    private static PercentageConverter _instance;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return System.Convert.ToDouble(value, culture) * (System.Convert.ToDouble(parameter, culture) / 100);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return System.Convert.ToDouble(value, culture) / (System.Convert.ToDouble(parameter, culture) / 100);
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return _instance ?? (_instance = new PercentageConverter());
    }
  }
}
