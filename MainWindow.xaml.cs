using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DumaVoteCounter {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            Left = Properties.Settings.Default.positionX;
            Top = Properties.Settings.Default.positionY;
            Width = Properties.Settings.Default.windowWidth;
            Height = Properties.Settings.Default.windowHeight;
            voteAgainstTextBox.TextChanged += voteTextChange;
            voteAbstainedTextBox.TextChanged += voteTextChange;
        }

        private void Dragging(object sender, MouseButtonEventArgs e) {
            this.DragMove();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Properties.Settings.Default.positionX = Left;
            Properties.Settings.Default.positionY = Top;
            Properties.Settings.Default.windowWidth = Width;
            Properties.Settings.Default.windowHeight = Height;
            Properties.Settings.Default.Save();
        }

        private void Exit_Click(object sender, RoutedEventArgs e) {
            var response = MessageBox.Show("Выйти из программы?", "Выход...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response == MessageBoxResult.No) return;
            else Application.Current.Shutdown();
        }

        private void NewVote_Click(object sender, RoutedEventArgs e) {
            voteForTextBox.Text = peopleNumberTextBox.Text;
            voteAgainstTextBox.Text = "0";
            voteAbstainedTextBox.Text = "0";
        }

        private void InputOnlyDigits(object sender, TextCompositionEventArgs e) {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;

        }

        private void voteTextChange(object sender, TextChangedEventArgs e) {
            int voteAbstained = 0; int voteAgainst = 0; int peopleNumber = 0;
            bool havePeopleNumber = Int32.TryParse(peopleNumberTextBox.Text, out peopleNumber);
            bool haveVoteAgainst = Int32.TryParse(voteAgainstTextBox.Text, out voteAgainst);
            bool haveVoteAbstained = Int32.TryParse(voteAbstainedTextBox.Text, out voteAbstained);
            //MessageBox.Show(voteAbstained.ToString());
            voteForTextBox.Text = (peopleNumber - voteAgainst - voteAbstained).ToString();
        }


    }
}
