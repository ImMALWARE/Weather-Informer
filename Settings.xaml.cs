using System.Windows;

namespace Weather_Informer
{
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            C.IsChecked = !Data.UseFahrenheit;
            F.IsChecked = Data.UseFahrenheit;
            ru.IsChecked = Data.language.Equals("ru");
            en.IsChecked = Data.language.Equals("en");
            //tray.IsChecked = 
        }
    }
}
