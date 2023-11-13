using System.Windows;

namespace Weather_Informer
{
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            if (Data.language == "en") {
                TheWindow.Title = "Weather Informer - Settings";
                DegreesHint.Text = "Show temperature in degrees...";
                C.Content = "Celsius";
                F.Content = "Farenheit";
                LanguageHint.Text = "Interface language: ";
                ru.Content = "Russian";
                en.Content = "English";
                tray.Content = "Enable notifications";
                NotificationsHint.Text = "If enabled, app will be minimized in tray when closed";
                save.Content = "Save";
            }
            C.IsChecked = !Data.UseFahrenheit;
            F.IsChecked = Data.UseFahrenheit;
            ru.IsChecked = Data.language.Equals("ru");
            en.IsChecked = Data.language.Equals("en");
            tray.IsChecked = Data.tray;
        }

        private void Save(object sender, RoutedEventArgs e) {
            Data.UseFahrenheit = !C.IsChecked.Value;
            if (C.IsChecked.Value) Data.UseFahrenheit = false;
            if (F.IsChecked.Value) Data.UseFahrenheit = true;
            if (ru.IsChecked.Value) Data.language = "ru";
            if (en.IsChecked.Value) Data.language = "en";
            Data.tray = tray.IsChecked.Value;
            Database.ExecAndLeave("UPDATE Settings SET UseFahrenheit=" + (Data.UseFahrenheit ? "1" : "0")+", Language=\"" + Data.language + "\", NotificationsEnabled=" + (Data.tray ? "1" : "0"));
            Close();
        }
    }
}
