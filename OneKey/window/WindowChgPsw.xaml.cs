using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using OneKey.src.util;

namespace OneKey.window
{
    /// <summary>
    /// WindowChgPsw.xaml 的交互逻辑
    /// </summary>
    public partial class WindowChgPsw : Window
    {
        public MainWindow mainWindow;
        public bool callClose = false;
        public WindowChgPsw()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (callClose)
            {
                return;
            }
            else
            {
                mainWindow.windowChgPsw = null;
                mainWindow.IsEnabled = true;
                mainWindow.TextBoxKey.Focus();
            }

        }

        private void ButtonYes_Click(object sender, RoutedEventArgs e)
        {
            string oldPsw = PswBoxOld.Password.ToString();
            string newPsw = PswBoxNew.Password.ToString();
            string secondPsw = PswBoxSecond.Password.ToString();
            if (string.IsNullOrWhiteSpace(newPsw))
            {
                MessageBox.Show("新密码不能为空");
                this.PswBoxOld.Focus();
                return;
            }
            if (!newPsw.Equals(secondPsw))
            {
                MessageBox.Show("两次输入的密码不相同");
                this.PswBoxOld.Focus();
                return;
            }
            string md5OldPsw = UMD5.strToSaltBase64Str(oldPsw);
            if (!UFile.validPsw(md5OldPsw))
            {
                MessageBox.Show("旧密码不正确");
                this.PswBoxOld.Focus();
                return;
            }

            UFile.encryptAllContentAndChgPsw(oldPsw, newPsw);
            this.callClose = true;
            this.Close();
            mainWindow.windowChgPsw = null;
            mainWindow.psw = newPsw;
            mainWindow.IsEnabled = true;
            mainWindow.TextBoxKey.Focus();
        }
    }
}
