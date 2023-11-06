using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Weather_Informer
{
    public partial class CityManager : Window
    {
        public CityManager()
        {
            InitializeComponent();
            CitiesList.SelectionChanged += SChangedHandler;
            foreach (KeyValuePair<int, string> city in Database.GetCities()) {
                CitiesList.Items.Add(new TextBlock { Text = city.Value, Foreground = new SolidColorBrush(Colors.White), FontSize = 20, Tag = city.Key });
            }
        }

        private void SelectAction(object sender, RoutedEventArgs e)
        {
            if (CitiesList.SelectedItem is TextBlock city) {
                Data.CityID = Convert.ToInt32(city.Tag);
                Data.CityFriendlyName = city.Text;
                Close();
            }
        }

        private void SChangedHandler(object sender, SelectionChangedEventArgs e)
        {
            SelectButton.IsEnabled = CitiesList.SelectedItem != null;
            SelectButton.Foreground = SelectButton.IsEnabled ? new SolidColorBrush(Colors.White) : (SolidColorBrush)(new BrushConverter().ConvertFrom("#666666"));
        }

        private void AddNewCity(object sender, RoutedEventArgs e) {
            AddCity.SelectedCityId = 0;
            new AddCity().ShowDialog();
            if (AddCity.SelectedCityId != 0) {
                Database.ExecAndLeave("INSERT INTO Cities VALUES (" + AddCity.SelectedCityId.ToString() + ", \"" + AddCity.SelectedCityName + "\")");
                CitiesList.Items.Add(new TextBlock {Text = AddCity.SelectedCityName, Foreground = new SolidColorBrush(Colors.White), FontSize = 20, Tag = AddCity.SelectedCityId});
            }
        }
    }
}
