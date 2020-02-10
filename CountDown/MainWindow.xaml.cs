using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;


// ToDo:
// * set target time via command line args
//     when not provided next quarter hour
//     optional: schedules for start times per weekday
// * optional: blink when over due

namespace CountDown
{
  /// <summary>
  /// Interaktionslogik für MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    //PerformanceCounter perfCPU = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
    System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
    private DateTime targetTime;

    public MainWindow()
    {
      InitializeComponent();

      mnuReset_Click(null, null);

      timer.Tick += new EventHandler(Timer_Tick);
      timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
      timer.Start();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
      TimeSpan remaining = targetTime.Subtract(DateTime.Now);
      if (DateTime.Now >= targetTime)
      {
        seconds.StartAngle = 0;
        seconds.EndAngle = 360;
        seconds.IndicatorBrush = new RadialGradientBrush(Colors.Yellow, Colors.Red);
        seconds.ProgressBorderBrush = new LinearGradientBrush(Colors.Yellow, Colors.Red, DateTime.Now.Millisecond * 3.6);
      }
      else if (remaining.TotalSeconds <= 60)
      {
        minutes.StartAngle = 0;
        minutes.EndAngle = 360;
        seconds.StartAngle = 0;
        seconds.EndAngle = DateTime.Now.Second * 6;
        minutes.IndicatorBrush = Brushes.Red;
        seconds.IndicatorBrush = new LinearGradientBrush(Colors.Red, Colors.Yellow, 0);
        txtTime.Foreground = Brushes.Orange;
      }
      else
      {
        minutes.StartAngle = DateTime.Now.Minute * 6;
        minutes.EndAngle = targetTime.Minute * 6;
        seconds.StartAngle = DateTime.Now.Second * 6 - 2;
        seconds.EndAngle = DateTime.Now.Second * 6 + 2;
        if (remaining.TotalMinutes < 2)
        {
          minutes.IndicatorBrush = Brushes.Yellow;
          seconds.IndicatorBrush = Brushes.Yellow;
          txtTime.Foreground = Brushes.Yellow;
        }
      }
      txtTime.Text = DateTime.Now.ToLongTimeString();
    }

    private void mnuReset_Click(object sender, RoutedEventArgs e)
    {
      targetTime = DateTime.Now;
      //targetTime = targetTime.Subtract(new TimeSpan(0, 0, targetTime.Minute % 15, targetTime.Second, targetTime.Millisecond)).AddMinutes(15);
      targetTime = targetTime.Subtract(new TimeSpan(0, 0, 0, targetTime.Second, targetTime.Millisecond)).AddMinutes(3);

      minutes.IndicatorBrush = Brushes.DarkGreen;
      seconds.IndicatorBrush = Brushes.Lime;
      seconds.ProgressBorderBrush = Brushes.DimGray;
      txtTime.Foreground = Brushes.Lime;
    }

    private void mnuClose_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
      if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
      {
        this.DragMove();
      }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      Properties.Settings.Default.WindowPos = new Rect(this.Left, this.Top, this.Width, this.Height);
      Properties.Settings.Default.Save();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.Left = Properties.Settings.Default.WindowPos.X;
      this.Top = Properties.Settings.Default.WindowPos.Y;
      this.Width = Properties.Settings.Default.WindowPos.Width;
      this.Height = Properties.Settings.Default.WindowPos.Height;
    }

    private void Window_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
      Window window = (Window)sender;
      window.Topmost = true;
    }
  }
}

