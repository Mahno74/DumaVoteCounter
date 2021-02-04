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
    /// Interaction logic for result.xaml
    /// </summary>
    public partial class ResultWindow : Window {

        public ResultWindow(Voting voting) {
            InitializeComponent();
            Left = Properties.Settings.Default.resultPositionX;
            Top = Properties.Settings.Default.resultPositionY;
            Width = Properties.Settings.Default.resultWindowWidth;
            Height = Properties.Settings.Default.resultWindowHeight;
            if (voting.Edinoglasno) {
                Edinoglasno();
            } else {
                ResultOfVoting(voting);
            }
        }

        private void Edinoglasno() {
            container_edinoglano.Visibility = Visibility.Visible;
            container_voteFor.Visibility = Visibility.Collapsed;
            container_voteAgainst.Visibility = Visibility.Collapsed;
            container_voteAbstained.Visibility = Visibility.Collapsed;
        }
        private void ResultOfVoting(Voting voting) {
            container_edinoglano.Visibility = Visibility.Collapsed;
            container_voteFor.Visibility = Visibility.Visible;
            container_voteAgainst.Visibility = Visibility.Visible;
            container_voteAbstained.Visibility = Visibility.Visible;
            lb_voteFor.Content = $"ЗА {voting.voteFor}";
            lb_voteAgainst.Content = $"ПРОТИВ {voting.voteAgainst}";
            lb_voteAbstained.Content = $"ВОЗДЕРЖАЛИСЬ {voting.voteAbstained}";
        }

        private void Result_Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Properties.Settings.Default.resultPositionX = Left;
            Properties.Settings.Default.resultPositionY = Top;
            Properties.Settings.Default.resultWindowWidth = Width;
            Properties.Settings.Default.resultWindowHeight = Height;
            Properties.Settings.Default.Save();
        }
        private void Dragging(object sender, MouseButtonEventArgs e) => this.DragMove();
    }
}
