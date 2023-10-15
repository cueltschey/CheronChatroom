using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SQLite;

namespace CheronChatroom
{
    public partial class MainWindow : Window
    {
        List<MessageData> messages = new List<MessageData>();

        public void WireUpDB()
        {
            messageDisplay.ItemsSource = messages;
        }

        public void ReadMessageList()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.dbpath))
            {
                conn.CreateTable<MessageData>();
                messages = (conn.Table<MessageData>().ToList()).OrderBy(c => c.Id).ToList();
            }
        }

        private void ScrollToBottom()
        {
            messageDisplay.ScrollIntoView(messageDisplay.Items[messageDisplay.Items.Count - 1]);
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void messageInput_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return && messageInput.Text != "")
            {
                MessageData message = new MessageData()
                {
                    Content = messageInput.Text,
                    Sender = "human"
                };

                using (SQLiteConnection conn = new SQLiteConnection(App.dbpath))
                {
                    conn.CreateTable<MessageData>();
                    conn.Insert(message);
                }
                ReadMessageList();
                WireUpDB();
                messageInput.Text = "";
                Debug.WriteLine(PythonForm.RunCheron());
                ScrollToBottom();
            }
        }
        private async void AutoLoad()
        {
            while(true)
            {
                ReadMessageList();
                WireUpDB();
                ScrollToBottom();
                Debug.WriteLine("Reloaded data...");
                await Task.Delay(1000);
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            ReadMessageList();
            WireUpDB();
            ScrollToBottom();
            AutoLoad();
        }
        
    }
}
