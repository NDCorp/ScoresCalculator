
using System;
using System.Windows;

namespace ScoreCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private VMScores vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new VMScores();
            DataContext = vm;
        }

        private void BtnCalculate_Click(object sender, RoutedEventArgs e)
        {
            Exception err = new Exception();

            if (!vm.GetScores(ref err))
                MessageBox.Show(err.Message, err.HResult.ToString(), MessageBoxButton.OK, MessageBoxImage.Error);
            else
                vm.CalculateScore();

            lstSection1.Items.Refresh();
            lstSection2.Items.Refresh();
            lstSection3.Items.Refresh();
        }
    }
}
