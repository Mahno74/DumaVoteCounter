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
using System.Drawing;
using System.IO;

namespace DumaVoteCounter {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Voting voting;
        public static bool fullscreen_resultWindow;
        ResultWindow resultWindow;
        public MainWindow() {
            InitializeComponent();
            Left = Properties.Settings.Default.positionX;
            Top = Properties.Settings.Default.positionY;
            Width = Properties.Settings.Default.windowWidth;
            Height = Properties.Settings.Default.windowHeight;
            tb_VoteAgainst.TextChanged += VoteTextChange;
            tb_voteAbstained.TextChanged += VoteTextChange;
            Reset_Click(null, null);
        }


        private void Reset_Click(object sender, RoutedEventArgs e) {
            tb_VoteFor.Text = tb_PeopleNumber.Text;
            tb_VoteAgainst.Text = "0";
            tb_voteAbstained.Text = "0";
            voting = new Voting(
                numberOfPeople: Convert.ToInt32(tb_PeopleNumber.Text), 
                voteFor: Convert.ToInt32(tb_PeopleNumber.Text), 
                voteAgainst: 0, 
                voteAbstained: 0);
            if (resultWindow != null) resultWindow.Close(); //закрваем второе окно если оно есть
            bt_SendResult.IsEnabled = true; //активация кнопки послать результат и всех полей
            tb_PeopleNumber.IsEnabled = true;
            tb_VoteFor.IsEnabled = true;
            tb_VoteAgainst.IsEnabled = true;
            tb_voteAbstained.IsEnabled = true;
            ScreenShot(false); //убираем скриншот
        }

        private void InputOnlyDigits(object sender, TextCompositionEventArgs e) {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;

        }

        private void VoteTextChange(object sender, TextChangedEventArgs e) {
            _ = Int32.TryParse(tb_PeopleNumber.Text, out int peopleNumber);
            _ = Int32.TryParse(tb_VoteAgainst.Text, out int voteAgainst);
            _ = Int32.TryParse(tb_voteAbstained.Text, out int voteAbstained);
            tb_VoteFor.Text = (peopleNumber - voteAgainst - voteAbstained).ToString(); //высчитываем ЗА на сновании полей против и воздежался
            _ = Int32.TryParse(tb_VoteFor.Text, out int voteFor);
            voting = new Voting(numberOfPeople: peopleNumber, voteFor: voteFor, voteAgainst: voteAgainst, voteAbstained: voteAbstained);

            bt_SendResult.IsEnabled = !voting.SomeThingWrong; //отключаем кнопку отсылки результата если неправильно заполненны поля
            bt_SendResult.Content = voting.Edinoglasno? "ЕДИНОГЛАСНО!" : "Отправить результат"; //меняем название кнопки в зависимости от результатов
        }

        private void SendResults_Click(object sender, RoutedEventArgs e) {
            resultWindow = new ResultWindow(voting);
            resultWindow.Show();
            ScreenShot(true);
            mainWindow.Focus(); //возвращаем фокус на главное окно для отработки хоткеев
            bt_SendResult.IsEnabled = false;
            tb_PeopleNumber.IsEnabled = false;
            tb_VoteFor.IsEnabled = false;
            tb_VoteAgainst.IsEnabled = false;
            tb_voteAbstained.IsEnabled = false;
        }

        private void Dragging(object sender, MouseButtonEventArgs e) => this.DragMove();
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (resultWindow != null) resultWindow.Close();
            Properties.Settings.Default.positionX = Left;
            Properties.Settings.Default.positionY = Top;
            Properties.Settings.Default.windowWidth = Width;
            Properties.Settings.Default.windowHeight = Height;
            Properties.Settings.Default.Save();
        }

        private void Vote_GotFocus(object sender, RoutedEventArgs e) {
            TextBox text = sender as TextBox;
            text.SelectAll();
        }

        private void ScreenShot(bool show) {
            switch (show) {
                case true:
                    if (resultWindow == null) return;
                    //resultWindow.Measure(new System.Windows.Size(300, 300));
                    //resultWindow.Arrange(new Rect(new System.Windows.Size(300, 300)));
                    RenderTargetBitmap bmp = new RenderTargetBitmap(Convert.ToInt32(resultWindow.Width), Convert.ToInt32(resultWindow.Height), 96, 96, PixelFormats.Pbgra32);
                    bmp.Render(resultWindow);
                    bottom_image.Source = bmp;
                    bottom_image.Height = 200;
                    //var encoder = new PngBitmapEncoder();
                    //encoder.Frames.Add(BitmapFrame.Create(bmp));
                    //using (Stream stm = File.Create(@"D:\12\test.png"))
                    //encoder.Save(stm);
                    break;
                case false:
                    bottom_image.Source = null;

                    break;
            }
        }

        #region Работа с контекстным меню
        private void MenuItem_Reset_Click(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.resultPositionX = 205;
            Properties.Settings.Default.resultPositionY = 0;
            Properties.Settings.Default.resultWindowWidth = 200;
            Properties.Settings.Default.resultWindowHeight = 200;
            mainWindow.Left = 0;
            mainWindow.Top = 0;
            mainWindow.Width = 200;
            mainWindow.Height = 400;
            menuItemFullScreenChecked.IsChecked = false;
            if (resultWindow != null) {
                resultWindow.Left = 201;
                resultWindow.Top = 0;
                resultWindow.Width = 200;
                resultWindow.Height = 200;
            } else {
                Properties.Settings.Default.resultPositionX = 201;
                Properties.Settings.Default.resultPositionY = 0;
                Properties.Settings.Default.resultWindowWidth = 200;
                Properties.Settings.Default.resultWindowHeight = 200;
                Properties.Settings.Default.Save();
            }
        }
        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e) {
            var response = MessageBox.Show("Выйти из программы?", "Выход...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response == MessageBoxResult.No) return;
            else Application.Current.Shutdown();
        }

        private void MenuItem_FullScreen_Checked(object sender, RoutedEventArgs e) {
            fullscreen_resultWindow = true;
            if (resultWindow != null) {
                resultWindow.WindowState = WindowState.Maximized;
            }
            //mainStackPanel.Orientation = Orientation.Horizontal;
            //mainStackPanel.VerticalAlignment = VerticalAlignment.Center;
        }
        private void MenuItem_FullScreen_Unchecked(object sender, RoutedEventArgs e) {
            fullscreen_resultWindow = false;
            if (resultWindow != null) {
                resultWindow.WindowState = WindowState.Normal;
            }

        }
        #endregion
    }
}
