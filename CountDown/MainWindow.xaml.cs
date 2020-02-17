using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

// ToDo:

namespace CountDown
{
  /// <summary>
  /// Interaktionslogik für MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    //PerformanceCounter perfCPU = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
    Dictionary<string, string> arguments = new Dictionary<string, string>();
    System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();
    private DateTime targetTime;
    private int   blinkCount = -1;
    private Brush radial = new RadialGradientBrush(Colors.Red, Colors.Orange);
    private Brush nextBrush;

    public MainWindow()
    {
      InitializeComponent();

      string[] args = Environment.GetCommandLineArgs();
      for (int index = 1; index < args.Length; index += 2)
      {
        arguments.Add(args[index], args[index + 1]);
      }

      mnuReset_Click(null, null);

      timer.Tick += new EventHandler(Timer_Tick);
      timer.Interval = new TimeSpan(0, 0, 0, 0, Properties.Settings.Default.UpdateInterval);
      timer.Start();
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
      TimeSpan remaining = targetTime.Subtract(DateTime.Now);
      if (DateTime.Now >= targetTime)
      {
        minutes.StartAngle = 0;
        minutes.EndAngle = 360;
        seconds.StartAngle = 0;
        seconds.EndAngle = 360;

        switch (blinkCount++)
        {
          case 0:
            seconds.ProgressBorderBrush = nextBrush;
            break;
          case 1:
            seconds.IndicatorBrush = seconds.ProgressBorderBrush;
            break;
          case 2:
            minutes.IndicatorBrush = seconds.IndicatorBrush;
            break;
          default:
            nextBrush = (nextBrush == Brushes.Red) ? radial : Brushes.Red;
            blinkCount = 0;
            break;
        }
      }
      else if (remaining.TotalSeconds <= 60)
      {
        minutes.StartAngle = 0;
        minutes.EndAngle = 360;
        seconds.StartAngle = 0;
        seconds.EndAngle = DateTime.Now.Second * 6;
        minutes.IndicatorBrush = 
        seconds.IndicatorBrush = new SolidColorBrush(Color.FromRgb(255, (byte)(240 - DateTime.Now.Second * 4), 0));
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
      string dt;
      if (!(arguments.TryGetValue(DateTime.Now.ToString("dddd"), out dt) && DateTime.TryParse(dt, out targetTime)))  
      {
        targetTime = DateTime.Now;
        targetTime = targetTime.Subtract(new TimeSpan(0, 0, targetTime.Minute % 15, targetTime.Second, targetTime.Millisecond)).AddMinutes(15);
      }

      minutes.IndicatorBrush = Brushes.Green;
      seconds.IndicatorBrush = Brushes.Lime;
      seconds.ProgressBorderBrush = Brushes.Transparent;
      txtTime.Foreground = Brushes.Lime;
    }

    private void mnuClose_Click(object sender, RoutedEventArgs e)
    {
      this.Close();
    }

    private void mnuSettings_Click(object sender, RoutedEventArgs e)
    {
      //MessageBox.Show(Path.GetDirectoryName(ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoaming).FilePath));
      System.Diagnostics.Process.Start("notepad.exe", ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.PerUserRoamingAndLocal).FilePath); 
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

