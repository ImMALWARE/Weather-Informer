using System.Windows;

namespace Weather_Informer
{
    public partial class CityManager : Window
    {
        public CityManager()
        {
            InitializeComponent();

        }

        private void SelectAction(object sender, RoutedEventArgs e)
        {

        }

        private void AddNewCity(object sender, RoutedEventArgs e)
        {
            new AddCity().ShowDialog();
        }
    }
}
