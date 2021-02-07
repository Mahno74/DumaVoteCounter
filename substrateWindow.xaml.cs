using System;
using System.Windows;


namespace DumaVoteCounter {
    public partial class SubstrateWindow : Window {
        public SubstrateWindow() {
            InitializeComponent();
            //берем данные положения из окна результатов
            Left = Properties.Settings.Default.resultPositionX;
            Top = Properties.Settings.Default.resultPositionY;
            Width = Properties.Settings.Default.resultWindowWidth;
            Height = Properties.Settings.Default.resultWindowHeight;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Maximized;
            lb_SessionNumber.Content = $"Заседание № {Properties.Settings.Default.sessionNumber}";
            lb_DateNow.Content = DateTime.Now.Date.ToShortDateString();
        }
    }
}
