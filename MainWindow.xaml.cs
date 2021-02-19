using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DumaVoteCounter {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        Voting voting;
        //public static bool fullscreen_resultWindow;
        ResultWindow resultWindow;
        SubstrateWindow subWindow;
        public MainWindow() {
            InitializeComponent();
            Left = Properties.Settings.Default.positionX;
            Top = Properties.Settings.Default.positionY;
            Width = Properties.Settings.Default.windowWidth;
            Height = Properties.Settings.Default.windowHeight;
            menuItemFullScreenChecked.IsChecked = Properties.Settings.Default.fullScreen;
            menuItemFullShowTotal.IsChecked = Properties.Settings.Default.showTotal;
            EventSubscriptions(); //подписываемся на события
            tb_SessionNumber.Text = Properties.Settings.Default.sessionNumber;
            Reset_Click(null, null);
        }

        private void EventSubscriptions() {
            //изменения текта в полях ПРОТИВ и ВОЗДЕРЖАЛИСЬ
            tb_VoteAgainst.TextChanged += VotesAgainsAbdstainetTextChange;
            tb_VoteAbstained.TextChanged += VotesAgainsAbdstainetTextChange;
            tb_VoteFor.TextChanged += VotesForTextChange;
            tb_SessionNumber.TextChanged += SessionNumberTextChange;
            //колесико мыши в полях ПРОТИВ и ВОЗДЕРЖАЛИСЬ
            lb_VoteAgainst.MouseWheel += VotesScroll;
            tb_VoteAgainst.MouseWheel += VotesScroll;
            lb_VoteAbstained.MouseWheel += VotesScroll;
            tb_VoteAbstained.MouseWheel += VotesScroll;
            tb_SessionNumber.MouseWheel += VotesScroll;
            //Кнопки плюс - минус на общем количествк депутатов на заседании
            lb_Minus.MouseDown += Add_Substract_Deputies_Click;
            lb_Plus.MouseDown += Add_Substract_Deputies_Click;

        }
        //Прячем и прказываем основное окно
        private void Hide_Click(object sender, RoutedEventArgs e) {
            
            if (Settings.hide) {
                bt_Hide.Content = new MaterialDesignThemes.Wpf.PackIcon { Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowUpCircle };
                MaxHeight = double.PositiveInfinity; MinHeight = Settings.minHeight;
                Height = Properties.Settings.Default.windowHeight;

            } else {
                Settings.minHeight = MinHeight;
                bt_Hide.Content = new MaterialDesignThemes.Wpf.PackIcon { Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowDownCircle };
                Properties.Settings.Default.windowHeight = Height;
                MinHeight = bt_Hide.Height - bt_Hide.Margin.Bottom;
                MaxHeight = bt_Hide.Height - bt_Hide.Margin.Bottom;
                Height = bt_Hide.Height - bt_Hide.Margin.Bottom;
            }
            Settings.hide = !Settings.hide;
        }



        private void InputOnlyDigits(object sender, TextCompositionEventArgs e) {
            if (!Char.IsDigit(e.Text, 0)) e.Handled = true;
        }

        private void SessionNumberTextChange(object sender, TextChangedEventArgs e) {
            Properties.Settings.Default.sessionNumber = tb_SessionNumber.Text;
            //Properties.Settings.Default.Save();

        }
        //изменение текст бокса с общим числом депутатов
        private void VotesForTextChange(object sender, TextChangedEventArgs e) {

            _ = Int32.TryParse(tb_VoteFor.Text, out int voteFor);
            _ = Int32.TryParse(tb_VoteAgainst.Text, out int voteAgainst);
            _ = Int32.TryParse(tb_VoteAbstained.Text, out int voteAbstained);

            if (voteAgainst == 0 & voteAbstained == 0) {
                Settings.peopleNumber = voteFor;
            } 
        }
        //изменение текст боксов с Против - Воздержались
        private void VotesAgainsAbdstainetTextChange(object sender, TextChangedEventArgs e) {

            _ = Int32.TryParse(tb_VoteAgainst.Text, out int voteAgainst);
            _ = Int32.TryParse(tb_VoteAbstained.Text, out int voteAbstained);

            voting = new Voting(voteAgainst, voteAbstained); //создаем объект голосование

            if (voting.Edinoglasno) {
                Settings.peopleNumber = voting.VoteFor; 
                tb_VoteFor.IsReadOnly = false;
                tb_VoteFor.Background = Brushes.LightGray;
                lb_VoteFor.Background = Brushes.LightGray;
                lb_Minus.Background = Brushes.LightGray;
                lb_Plus.Background = Brushes.LightGray;
                lb_VoteFor.Content = "ПРИСУТСТВУЕТ";
                lb_Plus.IsEnabled  = true;
                lb_Minus.IsEnabled = true;
            } else {
                tb_VoteFor.IsReadOnly = true;
                lb_VoteFor.Content = "-ЗА-";
                tb_VoteFor.Background = Brushes.Green;
                lb_VoteFor.Background = Brushes.Green;
                lb_Minus.Background = Brushes.Green;
                lb_Plus.Background = Brushes.Green;
                lb_Plus.IsEnabled = false;
                lb_Minus.IsEnabled = false;
            }
            tb_VoteFor.Text = voting.VoteFor.ToString(); //получаем и публикуем расчитанное поле ЗА

            bt_SendResult.IsEnabled = !voting.SomeThingWrong; //отключаем кнопку отсылки результата если неправильно заполненны поля
            bt_SendResult.Content = voting.Edinoglasno ? "ЕДИНОГЛАСНО!" : "ОТПРАВИТЬ"; //меняем название кнопки в зависимости от результатов
        }

        //Изменение количества присутвующих щелчками мыши
        private void Add_Substract_Deputies_Click(object sender, MouseButtonEventArgs e) {
            _ = Int32.TryParse(tb_VoteFor.Text, out int isPresent);
            string sender_name = (sender as Control).Name;
            switch (sender_name) {
                case "lb_Minus":
                    if (isPresent > 0)
                        tb_VoteFor.Text = (--isPresent).ToString();
                    break;
                case "lb_Plus":
                    if (isPresent < Settings.MaxNumberOfDeputies)
                        tb_VoteFor.Text = (++isPresent).ToString();
                    break;
                default:
                    break;
            }
        }

        private void SendResults_Click(object sender, RoutedEventArgs e) {
            resultWindow = new ResultWindow(voting);
            resultWindow.Show();
            ScreenShot(true);
            mainWindow.Focus(); //возвращаем фокус на главное окно для отработки хоткеев
            //деактивируем елементы управления
            bt_SendResult.IsEnabled = false;
            tb_VoteFor.IsEnabled = false;
            tb_VoteAgainst.IsEnabled = false;
            tb_VoteAbstained.IsEnabled = false;
            lb_Minus.IsEnabled = false;
            lb_Plus.IsEnabled = false;
            lb_Minus.Opacity = 0.6;
            lb_Plus.Opacity = 0.6;
        }
        private void Reset_Click(object sender, RoutedEventArgs e) {
            tb_VoteFor.Text = Settings.peopleNumber.ToString();
            tb_VoteAgainst.Text = "0";
            tb_VoteAbstained.Text = "0";
            voting = new Voting();
            if (resultWindow != null) resultWindow.Close(); //закрваем второе окно если оно есть
            resultWindow = null;
            ScreenShot(false); //убираем скриншот
            //Активируем элементы управления
            bt_SendResult.IsEnabled = true;
            tb_VoteFor.IsEnabled = true;
            tb_VoteAgainst.IsEnabled = true;
            tb_VoteAbstained.IsEnabled = true;
            lb_Minus.IsEnabled = true;
            lb_Plus.IsEnabled = true;
            lb_Minus.Opacity = 1;
            lb_Plus.Opacity = 1;
        }
        
        private void Dragging(object sender, MouseButtonEventArgs e) => this.DragMove();
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (resultWindow != null) resultWindow.Close();
            if (subWindow != null) subWindow.Close();
            Properties.Settings.Default.positionX = Left;
            Properties.Settings.Default.positionY = Top;
            Properties.Settings.Default.windowWidth = Width;
            Properties.Settings.Default.windowHeight = Height;
            Properties.Settings.Default.sessionNumber = tb_SessionNumber.Text;
            Properties.Settings.Default.fullScreen = menuItemFullScreenChecked.IsChecked;
            Properties.Settings.Default.Save();
        }

        //выбираем весь тескт в поле ПРОТИВ или ВОЗДЕРЖАЛИСЬ при работе с клавиатуры
        private void Vote_GotFocus(object sender, RoutedEventArgs e) {
            TextBox text = sender as TextBox;
            text.SelectAll();
        }

        //показываем или не показываем скриншот с итогами голосования
        private void ScreenShot(bool show) {
            switch (show) {
                case true:
                    if (resultWindow == null) return;
                    RenderTargetBitmap bmp = new RenderTargetBitmap(Convert.ToInt32(resultWindow.Width), Convert.ToInt32(resultWindow.Height), 96, 96, PixelFormats.Pbgra32);
                    bmp.Render(resultWindow);
                    bottom_image.Source = bmp;
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
            menuItemFullScreenChecked.IsChecked = false;
            mainWindow.Left = 0;
            mainWindow.Top = 0;
            mainWindow.Width = MinWidth;
            mainWindow.Height = MinHeight;
            menuItemFullScreenChecked.IsChecked = false;
            if (resultWindow != null) {
                resultWindow.Left = MinWidth +1;
                resultWindow.Top = 0;
                resultWindow.Width = 200;
                resultWindow.Height = 200;
            } else {
                Properties.Settings.Default.resultPositionX = MinWidth + 1;
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

        //private void menuItemFullScreenChecked_Click(object sender, RoutedEventArgs e) {
        //    Settings.fullscreen_resultWindow = menuItemFullScreenChecked.IsChecked;
        //    if (Settings.fullscreen_resultWindow) {
        //        if (resultWindow != null) {
        //            resultWindow.WindowState = WindowState.Maximized;
        //        }
        //    } else {
        //        if (resultWindow != null) {
        //            resultWindow.WindowState = WindowState.Normal;
        //        }
        //    }
        //}
        private void MenuItem_FullScreen_Checked(object sender, RoutedEventArgs e) {
            Settings.fullscreen_resultWindow = true;
            if (resultWindow != null) {
                resultWindow.WindowState = WindowState.Maximized;
            }
        }
        private void MenuItem_FullScreen_Unchecked(object sender, RoutedEventArgs e) {
            Settings.fullscreen_resultWindow = false;
            if (resultWindow != null) {
                resultWindow.WindowState = WindowState.Normal;
            }

        }
        private void MenuItemSubstrateWindow_Checked(object sender, RoutedEventArgs e) {
            if (subWindow == null) {
                subWindow = new SubstrateWindow();
                subWindow.Show();
            }
        }

        private void MenuItemSubstrateWindow_Unchecked(object sender, RoutedEventArgs e) {
            if (subWindow != null) {
                subWindow.Close();
                subWindow = null;
            }
        }
        private void MenuItemFullShowTotal_Click(object sender, RoutedEventArgs e) {
            Properties.Settings.Default.showTotal = menuItemFullShowTotal.IsChecked;
        }

        private void About_Click(object sender, RoutedEventArgs e) {
            AboutWindow aw = new AboutWindow();
            aw.ShowDialog();
        }
        #endregion


        #region Работа с колесом мыши
        //Меняем количество голосов с помощью колеса мыши
        private void VotesScroll(object sender, MouseWheelEventArgs e) {
            string sender_name = (sender as Control).Name;

            //меняем колесиком ПРОТИВ
            if (sender_name == lb_VoteAgainst.Name || sender_name == tb_VoteAgainst.Name) {
                _ = Int32.TryParse(tb_VoteFor.Text, out int voteFor);
                _ = Int32.TryParse(tb_VoteAgainst.Text, out int votes);

                if (e.Delta > 0 && voteFor > 0) votes++;
                if (e.Delta < 0 && votes > 0) votes--;

                tb_VoteAgainst.Text = votes.ToString();
            }

            //меняем колесиком ВОЗДЕРЖАЛИСЬ
            if (sender_name == lb_VoteAbstained.Name || sender_name == tb_VoteAbstained.Name) {
                _ = Int32.TryParse(tb_VoteFor.Text, out int voteFor);
                _ = Int32.TryParse(tb_VoteAbstained.Text, out int votes);

                if (e.Delta > 0 && voteFor > 0) votes++;
                if (e.Delta < 0 && votes > 0) votes--;

                tb_VoteAbstained.Text = votes.ToString();
            }

            //меняем колесиком номер заседания
            if (sender_name == tb_SessionNumber.Name) {
                _ = Int32.TryParse(tb_SessionNumber.Text, out int session);

                if (e.Delta > 0) session++;
                if (e.Delta < 0 && session > 1) session--;

                tb_SessionNumber.Text = session.ToString();
            }
        }
        #endregion

    }
}
