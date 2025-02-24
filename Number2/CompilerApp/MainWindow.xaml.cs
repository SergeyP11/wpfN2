using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CompilerApp
{
    public partial class MainWindow : Window
    {
        private readonly Registers _registers = new Registers();
        private readonly Compiler _compiler = new Compiler();
        private readonly Interpreter _interpreter;

        public MainWindow()
        {
            InitializeComponent();
            _interpreter = new Interpreter(_registers);
        }

        private async void CompileAndRunButton_Click(object sender, RoutedEventArgs e)
        {
            await ExecuteCode(CodeTextBox.Text);
        }

        private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string code = await File.ReadAllTextAsync(openFileDialog.FileName);
                    await ExecuteCode(code);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private async Task ExecuteCode(string code)
        {
            LoadingDialog loadingDialog = new LoadingDialog();
            loadingDialog.Show();

            string result = "";
            MemoryStream compiledCode = null;
            await Task.Run(() =>
            {
                try
                {
                    Dispatcher.Invoke(() => loadingDialog.SetStatus("Compiling..."));
                    _registers.ClearRegisters();
                    compiledCode = _compiler.Compile(code);  

                    Dispatcher.Invoke(() => loadingDialog.SetStatus("Executing..."));
                    using (compiledCode) //using для удаления потока после интерпретации.
                    {
                        result = _interpreter.Execute(compiledCode);
                    }
                }
                catch (Exception ex)
                {
                    result = $"Error: {ex.Message}";
                }
                finally
                {
                    Dispatcher.Invoke(() =>
                    {
                        OutputTextBox.Text = result;
                        loadingDialog.Close();
                    });
                }
            });
        }

    }
}