using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DumaVoteCounter {
    public partial class ResultWindow : Window {

        public ResultWindow(Voting voting) {
            InitializeComponent();
            Left = Properties.Settings.Default.resultPositionX;
            Top = Properties.Settings.Default.resultPositionY;
            Width = Properties.Settings.Default.resultWindowWidth;
            Height = Properties.Settings.Default.resultWindowHeight;
            if (voting.Edinoglasno) {
                Result_Edinoglasno();
            } else {
                Result_NOT_Edinoglasno(voting);
            }
            //показывать или не показывать общее количество присутвующих депутатов
            rowTotal.Height = Properties.Settings.Default.showTotal ? new GridLength(0.5, GridUnitType.Star) : new GridLength(0, GridUnitType.Star);
            lb_total.Content = $"Присутвует депутатов - {Settings.peopleNumber}  ({voting.PercentOfAttendance})";
        }

        private void Result_Edinoglasno() {
            container_lb_edinoglano.Visibility = Visibility.Visible;
            container_lb_voteFor.Visibility = Visibility.Collapsed;
            container_lb_voteAgainst.Visibility = Visibility.Collapsed;
            container_lb_voteAbstained.Visibility = Visibility.Collapsed;
            container_lb_accepted.Background = Brushes.LightGreen;
            lb_accepted.Content = "РЕШЕНИЕ ПРИНЯТО";
        }



        private void Result_NOT_Edinoglasno(Voting voting) {
            if (voting.Accepted) {
                container_lb_accepted.Background = Brushes.LightGray;
                lb_accepted.Content = "РЕШЕНИЕ ПРИНЯТО";
            } else {
                container_lb_accepted.Background = Brushes.LightGray;
                lb_accepted.Content = "РЕШЕНИЕ НЕ ПРИНЯТО";
                lb_accepted.Foreground = Brushes.Red;
                container_lb_accepted.BorderBrush = Brushes.Red;
                container_lb_accepted.BorderThickness = new Thickness(7);
            }
            container_lb_edinoglano.Visibility = Visibility.Collapsed;
            container_lb_voteFor.Visibility = Visibility.Visible;
            container_lb_voteAgainst.Visibility = Visibility.Visible;
            container_lb_voteAbstained.Visibility = Visibility.Visible;
            lb_voteFor.Content = $"ЗА - {voting.VoteFor}";
            lb_voteAgainst.Content = $"ПРОТИВ - {voting.voteAgainst}";
            lb_voteAbstained.Content = $"ВОЗДЕРЖАЛИСЬ - {voting.voteAbstained}";
        }

        private void Result_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            //не сохраняем размеры доп экрана если выбран полнооконный режим
            if (!Settings.fullscreen_resultWindow) {
                //если был выбран полноэкранных режим то не сохраняем размены окна
                Properties.Settings.Default.resultWindowWidth = Width;
                Properties.Settings.Default.resultWindowHeight = Height;
            }
            Properties.Settings.Default.resultPositionX = Left;
            Properties.Settings.Default.resultPositionY = Top;
            Properties.Settings.Default.Save();
        }
        private void Dragging(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            //вспоминаем был ли выбран полный или не полный экран при последнем запуске окна
            WindowState = Settings.fullscreen_resultWindow ? WindowState.Maximized : WindowState.Normal;
        }

        //регулировка размера окна колесиком мыши
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e) {
            if (e.Delta > 0) {
                Width += 25;
                Height += 25;
            } else {
                Width -= 25;
                Height -= 25;
            }
        }
    }
}
