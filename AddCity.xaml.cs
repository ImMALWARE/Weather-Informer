using System.Net.Http;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Newtonsoft.Json.Linq;
using System;
using System.Windows.Input;

namespace Weather_Informer {
    public partial class AddCity : Window {
        public static int SelectedCityId = 0;
        public static string SelectedCityName = null;

        public AddCity() {
            InitializeComponent();
            SearchResults.SelectionChanged += SChangedHandler;
            PreviewKeyDown += new KeyEventHandler((object sender, KeyEventArgs e) => {if (e.Key == Key.Escape) Close();});
        }

        private void SelectAction(object sender, RoutedEventArgs e) {
            if (SearchResults.SelectedItem is TextBlock city) {
                SelectedCityId = Convert.ToInt32(city.Tag);
                SelectedCityName = city.Text;
                Close();
            }
        }

        private async void Search(object sender, RoutedEventArgs e) {
            SelectButton.IsEnabled = false;
            SearchResults.Items.Clear();
            using (var httpClient = new HttpClient()) {
                HttpResponseMessage response = await httpClient.GetAsync("https://api.openweathermap.org/data/2.5/find?q="+SearchBox.Text+"&appid="+Data.token+"&lang=ru");
                var result = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
                if (result["cod"].ToString() != "200") {
                    MessageBox.Show(result.ToString(), "Добавление города", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                
                if (result["count"].ToString() == "0") {
                    MessageBox.Show("Не найдены города по этому запросу!", "Добавление города", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                foreach (var city in result["list"]) {
                    SearchResults.Items.Add(new TextBlock {Text = city["name"].ToString() + ", "+ city["sys"]["country"].ToString(), Foreground = new SolidColorBrush(Colors.White), FontSize = 20, Tag = city["id"].ToString()});
                }     
            }
        }
        private void SChangedHandler(object sender, SelectionChangedEventArgs e) {
            SelectButton.IsEnabled = SearchResults.SelectedItem != null;
            SelectButton.Foreground = SelectButton.IsEnabled ? new SolidColorBrush(Colors.White) : (SolidColorBrush)(new BrushConverter().ConvertFrom("#666666"));
        }

    }
}
