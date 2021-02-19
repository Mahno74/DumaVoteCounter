using System.Windows;
using System.Windows.Input;

namespace DumaVoteCounter {
    public partial class AboutWindow : Window {
        public AboutWindow() => InitializeComponent();
        private void Dragging(object sender, MouseButtonEventArgs e) => DragMove();
        private void Close(object sender, RoutedEventArgs e) => Close();
    }
}
