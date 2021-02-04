using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        Voting voting;
        //bool resultShow;
        ResultWindow res;
        public MainWindow() {
            InitializeComponent();
            Left = Properties.Settings.Default.positionX;
            Top = Properties.Settings.Default.positionY;
            Width = Properties.Settings.Default.windowWidth;
            Height = Properties.Settings.Default.windowHeight;
            voteAgainstTextBox.TextChanged += VoteTextChange;
            voteAbstainedTextBox.TextChanged += VoteTextChange;
            Reset_Click(null, null);
        }


        private void Reset_Click(object sender, RoutedEventArgs e) {
            voteForTextBox.Text = peopleNumberTextBox.Text;
            voteAgainstTextBox.Text = "0";
            voteAbstainedTextBox.Text = "0";
            voting = new Voting(
                numberOfPeople: Convert.ToInt32(peopleNumberTextBox.Text), 
                voteFor: Convert.ToInt32(peopleNumberTextBox.Text), 
                voteAgainst: 0, 
                voteAbstained: 0);
            if (res != null) res.Close(); //закрваем второе окно если оно есть
            bt_SendResult.IsEnabled = true; //активация кнопки послать результат и всех полей
            peopleNumberTextBox.IsEnabled = true;
            voteAgainstTextBox.IsEnabled = true;
            voteAbstainedTextBox.IsEnabled = true;
        }

        private void InputOnlyDigits(object sender, TextCompositionEventArgs e) {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;

        }

        private void VoteTextChange(object sender, TextChangedEventArgs e) {
            _ = Int32.TryParse(peopleNumberTextBox.Text, out int peopleNumber);
            _ = Int32.TryParse(voteAgainstTextBox.Text, out int voteAgainst);
            _ = Int32.TryParse(voteAbstainedTextBox.Text, out int voteAbstained);
            voteForTextBox.Text = (peopleNumber - voteAgainst - voteAbstained).ToString();
            _ = Int32.TryParse(voteForTextBox.Text, out int voteFor);
            voting = new Voting(numberOfPeople: peopleNumber, voteFor: voteFor, voteAgainst: voteAgainst, voteAbstained: voteAbstained);
            bt_SendResult.IsEnabled = !voting.SomeThingWrong; //отключаем кнопку отсылки результата если неправильно заполненны поля
            bt_SendResult.Content = voting.Edinoglasno? "ЕДИНОГЛАСНО!" : "Отправить результат"; //меняем название кнопки в зависимости от результатов
        }

        private void SendResults_Click(object sender, RoutedEventArgs e) {
            res = new ResultWindow(voting);
            res.Show();
            mainWindow.Focus(); //возвращаем фокус на главное окно для отработки хоткеев
            bt_SendResult.IsEnabled = false;
            peopleNumberTextBox.IsEnabled = false;
            voteAgainstTextBox.IsEnabled = false;
            voteAbstainedTextBox.IsEnabled = false;

        }

        private void Dragging(object sender, MouseButtonEventArgs e) => this.DragMove();
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (res != null) res.Close();
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

        private void Vote_GotFocus(object sender, RoutedEventArgs e) {
            TextBox text = sender as TextBox;
            text.SelectAll();
        }
    }
}
