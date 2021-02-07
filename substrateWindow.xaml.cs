using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DumaVoteCounter {
    /// <summary>
    /// Interaction logic for Ssubstrate.xaml
    /// </summary>
    public partial class SubstrateWindow : Window {
        public SubstrateWindow() {
            InitializeComponent();
            Left = Properties.Settings.Default.resultPositionX;
            Top = Properties.Settings.Default.resultPositionY;
            Width = Properties.Settings.Default.resultWindowWidth;
            Height = Properties.Settings.Default.resultWindowHeight;
            //WindowState = WindowState.Maximized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Maximized;
            lb_SessionNumber.Content = $"Заседание № {Properties.Settings.Default.sessionNumber}";
            lb_DateNow.Content = DateTime.Now.Date.ToShortDateString();
        }
    }
}
