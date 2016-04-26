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
using System.Windows.Navigation;
using System.Windows.Shapes;
using OneKey.window;
using OneKey.src.util;
using System.Text.RegularExpressions;
using System.Windows.Threading;
using System.Threading;

using System.Windows.Forms;

namespace OneKey
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public string psw = string.Empty;
        private DispatcherTimer txtChgTimer = new DispatcherTimer();
        private bool txtChgValid = true;

        public WindowAbout windowAbout;

        public MainWindow()
        {
            InitializeComponent();
            txtChgTimer.Tick += txtChgTimer_Tick;
            txtChgTimer.Interval = TimeSpan.FromMilliseconds(10);
            txtChgTimer.Start();
        }

        private Thread loadTitlesThread = null;
        private void loadTitlesThreadRun()
        {
            titles = UFile.getAllTitles();
            setTextBoxKey("");
            setTextBoxContent("");
            setListBoxTitles();
            setButtonAdd(false);
            setButtonChg(false);
            setButtonDel(false);
            setTextBoxKeyFocus();
        }
        public void startLoadTitlesThread()
        {
            loadTitlesThread = new Thread(loadTitlesThreadRun);
            loadTitlesThread.IsBackground = true;
            loadTitlesThread.Start();
        }
        private void txtChgTimer_Tick(object sender, EventArgs e)
        {
            if (txtChgValid == false && (loadTitlesThread == null || loadTitlesThread.ThreadState != ThreadState.Running)) 
            {
                
                string txt = TextBoxKey.Text;
                titles = UFile.getTitlesByKey(txt);
                setListBoxTitles();
                setButtonAdd(false);
                setButtonChg(false);
                setButtonDel(false);
                setTextBoxContent("");
                txtChgValid = true;
            }
        }
        private delegate void setListBoxTitlesGate();
        private string[] titles=new string[0];
        private void setListBoxTitles()
        {
            if (ListBoxTitles.Dispatcher.Thread != Thread.CurrentThread)
            {
                setListBoxTitlesGate sg = new setListBoxTitlesGate(setListBoxTitles);
                Dispatcher.Invoke(sg);
            }
            else
            {
                ListBoxTitles.ItemsSource =null;
                ListBoxTitles.ItemsSource = titles;
            }
        }
        private delegate void setTextBoxKeyGate(string text);
        private void setTextBoxKey(string text)
        {
            if (TextBoxKey.Dispatcher.Thread != Thread.CurrentThread)
            {
                setTextBoxKeyGate sg = new setTextBoxKeyGate(setTextBoxKey);
                Dispatcher.Invoke(sg, new object[] { text });
            }
            else
            {
                TextBoxKey.Text = text;
            }
        }
        private delegate void setButtonAddGate(bool flag);
        private void setButtonAdd(bool flag)
        {
            if (ButtonAdd.Dispatcher.Thread != Thread.CurrentThread)
            {
                setButtonAddGate sg = new setButtonAddGate(setButtonAdd);
                Dispatcher.Invoke(sg, new object[] { flag });
            }
            else
            {
                ButtonAdd.IsEnabled = flag;
            }
        }
        private delegate void setButtonChgGate(bool flag);
        private void setButtonChg(bool flag)
        {
            if (ButtonChg.Dispatcher.Thread != Thread.CurrentThread)
            {
                setButtonChgGate sg = new setButtonChgGate(setButtonChg);
                Dispatcher.Invoke(sg, new object[] { flag });
            }
            else
            {
                ButtonChg.IsEnabled = flag;
            }
        }
        private delegate void setButtonDelGate(bool flag);
        private void setButtonDel(bool flag)
        {
            if (ButtonDel.Dispatcher.Thread != Thread.CurrentThread)
            {
                setButtonDelGate sg = new setButtonDelGate(setButtonDel);
                Dispatcher.Invoke(sg, new object[] { flag });
            }
            else
            {
                ButtonDel.IsEnabled = flag;
            }
        }
        private delegate void setTextBoxKeyFocusGate();
        private void setTextBoxKeyFocus()
        {
            if (TextBoxKey.Dispatcher.Thread != Thread.CurrentThread)
            {
                setTextBoxKeyFocusGate sg = new setTextBoxKeyFocusGate(setTextBoxKeyFocus);
                Dispatcher.Invoke(sg);
            }
            else
            {
                TextBoxKey.Focus();
            }
        }
        private delegate void setTextBoxContentGate(string text);
        private void setTextBoxContent(string text)
        {
            if (TextBoxContent.Dispatcher.Thread != Thread.CurrentThread)
            {
                setTextBoxContentGate sg = new setTextBoxContentGate(setTextBoxContent);
                Dispatcher.Invoke(sg, new object[] { text });
            }
            else
            {
                TextBoxContent.Text = text;
            }
        }
        public WindowChgPsw windowChgPsw = null;
        private void MenuChgPsw_Click(object sender, RoutedEventArgs e)
        {
            windowChgPsw = new WindowChgPsw();
            windowChgPsw.mainWindow = this;
            windowChgPsw.Owner = this;
            this.IsEnabled = false;
            windowChgPsw.Show();
            windowChgPsw.PswBoxOld.Focus();
        }

        private void MenueExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        public WindowLogin windowLogin = null;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (!UFile.existsFile()||!UFile.existsPsw())
            {
                UFile.createFile();
                windowAddPsw = new WindowAddPsw();
                windowAddPsw.mainWindow = this;
                windowAddPsw.Owner = this;
                this.IsEnabled = false;
                windowAddPsw.Show();
                windowAddPsw.PswBoxFirst.Focus();
            }
            else
            {
                windowLogin = new WindowLogin();
                windowLogin.mainWindow = this;
                windowLogin.Owner = this;
                this.IsEnabled = false;
                windowLogin.Show();
                windowLogin.PswBox.Focus();
            }
        }
        public WindowAddPsw windowAddPsw = null;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (windowLogin != null)
            {
                windowLogin.callClose = true;
                windowLogin.Close();
            }
            if (windowAddPsw != null)
            {
                windowAddPsw.callClose = true;
                windowAddPsw.Close();
            }
            if (windowChgPsw != null)
            {
                windowChgPsw.callClose = true;
                windowChgPsw.Close();
            }
            if (windowAbout != null)
            {
                windowAbout.callClose = true;
                windowAbout.Close();
            }
        }
        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string text = TextBoxContent.Text;
                string[] contentText = Regex.Split(text, Environment.NewLine);
                if (contentText.Length < 2)
                {
                    System.Windows.MessageBox.Show("最少两行");
                    ButtonAdd.IsEnabled = false;
                    ButtonChg.IsEnabled = false;
                    TextBoxContent.Focus();
                    return;
                }
                foreach (string s in contentText)
                {
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        System.Windows.MessageBox.Show("不能有空行");
                        ButtonAdd.IsEnabled = false;
                        ButtonChg.IsEnabled = false;
                        TextBoxContent.Focus();
                        return;
                    }
                }
                if (UFile.existsTitle(contentText[0]))
                {
                    System.Windows.MessageBox.Show("条目已存在");
                    ButtonAdd.IsEnabled = false;
                    TextBoxContent.Focus();
                    return;
                }
                UFile.addDecryptContent(contentText, psw);
                startLoadTitlesThread();
            }
            catch (Exception ex)
            {
                TextBoxContent.Text = ex.Message;
            }
        }
        private void TextBoxKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtChgValid = false;
            ButtonAdd.IsEnabled = false;
            ButtonChg.IsEnabled = false;
            ButtonDel.IsEnabled = false;
            TextBoxContent.Text = "";
        }
        private void ButtonHide_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxContent.IsVisible)
            {
                TextBoxContent.Visibility = Visibility.Hidden;
                ButtonHide.Content = "显示";
            }
            else
            {
                TextBoxContent.Visibility = Visibility.Visible;
                ButtonHide.Content = "隐藏";
            }
            TextBoxKey.Focus();
        }
        private void ListBoxTitles_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxTitles.SelectedItem != null)
            {
                string title = ListBoxTitles.SelectedItem.ToString();
                string[] content = UFile.getByTitle(title);
                string[] texts = new string[content.Length - 1];
                texts[0] = content[0];
                for (int i = 2; i < content.Length; i++)
                {
                    string text = content[i];
                    string decryptText = UAES.decryptStrToStr(text, psw);
                    texts[i - 1] = decryptText;
                }
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < texts.Length; i++)
                {
                    if (i != texts.Length - 1)
                    {
                        sb.Append(texts[i]).Append(Environment.NewLine);
                    }
                    else
                    {
                        sb.Append(texts[i]);
                    }
                }
                TextBoxContent.Text = sb.ToString();
                ButtonDel.IsEnabled = true;
                ButtonAdd.IsEnabled = false;
                ButtonChg.IsEnabled = false;
            }
        }
        private void ButtonDel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mr = System.Windows.MessageBox.Show("确定删除？", "警告", MessageBoxButton.YesNo);
            if (mr == MessageBoxResult.No)
            {
                return;
            }
            string title = ListBoxTitles.SelectedItem.ToString();
            UFile.deleteByTitle(title);
            startLoadTitlesThread();
            ButtonDel.IsEnabled = false;
        }
        private void ButtonChg_Click(object sender, RoutedEventArgs e)
        {
            string contentText = TextBoxContent.Text;
            string[] contentLines = Regex.Split(contentText, Environment.NewLine);
            if (contentText.Length < 2)
            {
                System.Windows.MessageBox.Show("最少两行");
                ButtonAdd.IsEnabled = false;
                ButtonChg.IsEnabled = false;
                TextBoxContent.Focus();
                return;
            }
            foreach (string s in contentLines)
            {
                if (string.IsNullOrWhiteSpace(s))
                {
                    System.Windows.MessageBox.Show("不能有空行");
                    ButtonAdd.IsEnabled = false;
                    ButtonChg.IsEnabled = false;
                    TextBoxContent.Focus();
                    return;
                }
            }
            if(!contentLines[0].Equals(ListBoxTitles.SelectedItem.ToString()))
            {
                System.Windows.MessageBox.Show("不允许修改标题");
                ButtonChg.IsEnabled = false;
                return;
            }
            UFile.deleteByTitle(contentLines[0]);
            UFile.addDecryptContent(contentLines, psw);
            ButtonChg.IsEnabled = false;
            ButtonAdd.IsEnabled = false;
            TextBoxKey.Focus();
        }
        private void TextBoxContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            ButtonAdd.IsEnabled = true;
            if (ListBoxTitles.SelectedItem != null)
            {
                ButtonChg.IsEnabled = true;
            }
        }
        private void MenuImportFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "文本文件|*.txt";
            ofd.ShowDialog();
            if (string.IsNullOrWhiteSpace(ofd.FileName))
            {
                this.TextBoxKey.Focus();
                return;
            }
            string importFilePath = ofd.FileName;
            UFile.importFile(importFilePath, psw);
            startLoadTitlesThread();
            System.Windows.MessageBox.Show("导入成功");
        }
        private void MenuExportFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "文本文件|*.txt";
            sfd.ShowDialog();
            if (string.IsNullOrWhiteSpace(sfd.FileName))
            {
                this.TextBoxKey.Focus();
                return;
            }
            string exportFilePath = sfd.FileName;
            UFile.exportFile(exportFilePath,psw);
            System.Windows.MessageBox.Show("导出成功");
            this.TextBoxKey.Focus();
        }

        private void MenuAbout_Click(object sender, RoutedEventArgs e)
        {
            windowAbout = new WindowAbout();
            windowAbout.Owner = this;
            windowAbout.mainWindow = this;
            this.IsEnabled = false;
            windowAbout.Show();
        }

        private void TextBoxKey_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                if (ListBoxTitles.Items.Count != 0)
                {
                    setListBoxTitles();
                    ListBoxTitles.Focus();
                }
            }
        }

        private void ListBoxTitles_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (ListBoxTitles.SelectedIndex == 0)
                {
                    TextBoxKey.Focus();
                }
            }
        }
    }
}