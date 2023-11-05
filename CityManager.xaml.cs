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
            foreach (KeyValuePair<int, string> city in Database.GetCities()) {
                CitiesList.Items.Add(new TextBlock { Text = city.Value, Foreground = new SolidColorBrush(Colors.White), FontSize = 20, Tag = city.Key });
            }
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
