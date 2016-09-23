using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Infinity_Timer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer m_timer;
        private long m_duration;
        private TimeSpan m_timeElapsed;
        private string m_fileA, m_fileB;
        private readonly string m_cLongFormat = string.Format("D{0}", long.MaxValue.ToString().Length);

        public MainWindow()
        {
            InitializeComponent();

            // Timer
            m_timer = new DispatcherTimer();
            m_timer.Interval = new TimeSpan(0, 0, 0, 1);    // Count up every second
            m_timer.Tick += TimerTick;
            m_duration = 0;
            m_timeElapsed = new TimeSpan(0);

            // Directory
            string directory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            m_fileA = Path.Combine(directory, "timer_a.txt");
            m_fileB = Path.Combine(directory, "timer_b.txt");
        }

        private void TimerTick(object sender, EventArgs e)
        {
            m_duration += 1;
            TimeSpan duration = TimeSpan.FromSeconds(m_duration);
            string strTime = duration.ToString();
            tbTime.Text = strTime;
            if (m_duration % 2 != 0)
            {
                WriteFile(m_fileA, strTime);
            }
            else
            {
                WriteFile(m_fileB, strTime);
            }
        }

        private void WriteFile(string filePath, string timeFmt)
        {
            using (StreamWriter file = new StreamWriter(filePath))
            {
                file.WriteLine(m_duration.ToString(m_cLongFormat));
                file.WriteLine(timeFmt);
            }
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            btnGo.IsEnabled = false;
            tbDirectories.Foreground = Brushes.Black;
            btnGo.Foreground = Brushes.LightGray;
            m_timer.Start();

            DoubleAnimation windowResizer = new DoubleAnimation(ActualHeight, ActualHeight - btnGo.ActualHeight, TimeSpan.FromMilliseconds(600));
            DoubleAnimation buttonResizer = new DoubleAnimation(btnGo.ActualHeight, 0, TimeSpan.FromMilliseconds(600));
            BeginAnimation(Window.HeightProperty, windowResizer);
            btnGo.BeginAnimation(Button.HeightProperty, buttonResizer);
            rowButton.Height = GridLength.Auto;
        }
    }
}
