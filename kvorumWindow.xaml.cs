using System;
using System.Windows;


namespace DumaVoteCounter {
    public partial class KvorumWindow : Window {
        Voting voting;
        public KvorumWindow(Voting voting) {
            InitializeComponent();
            this.voting = voting;
            //берем данные положения из окна результатов
            Left = Properties.Settings.Default.resultPositionX;
            Top = Properties.Settings.Default.resultPositionY;
            Width = Properties.Settings.Default.resultWindowWidth;
            Height = Properties.Settings.Default.resultWindowHeight;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Maximized;
            lb_TimeNow.Content = DateTime.Now.ToShortTimeString();
            lb_Kvorum.Content = $"{Settings.peopleNumber}  ({voting.PercentOfAttendance})"; ;
        }
    }
}
