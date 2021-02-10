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
    /// Interaction logic for aboutWindow.xaml
    /// </summary>
    public partial class aboutWindow : Window {
        public aboutWindow() {
            InitializeComponent();
        }

        private void Dragging(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Close(object sender, RoutedEventArgs e) {
            this.Close();
        }
    }
}
