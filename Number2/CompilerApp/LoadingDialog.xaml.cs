using System.Windows;

namespace CompilerApp
{
    public partial class LoadingDialog : Window
    {
        public LoadingDialog()
        {
            InitializeComponent();
        }

        public void SetStatus(string status)
        {
            Dispatcher.Invoke(() => StatusTextBlock.Text = status);
        }
    }
}