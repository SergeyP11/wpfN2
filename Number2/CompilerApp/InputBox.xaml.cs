
using System.Windows;

namespace CompilerApp
{
    public partial class InputBox : Window
    {
        public string Answer { get; set; }
        public InputBox(string message, string title)
        {
            InitializeComponent();
            Title = title;
            MessageTextBlock.Text = message;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Answer = AnswerTextBox.Text;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}