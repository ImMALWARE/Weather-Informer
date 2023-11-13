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
        public static int SelectedCityId;
        public static string SelectedCityName;

        public AddCity() {
            SelectedCityId = 0;
            SelectedCityName = null;
            InitializeComponent();
            if (Data.language == "en") {
                TheWindow.Title = "Weather Informer - Adding new city";
                ChooseCity.Text = "Search for city name";
                SelectButton.Content = "Add";
            }
            
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
                HttpResponseMessage response = null;
                try {
                    response = await httpClient.GetAsync("https://api.openweathermap.org/data/2.5/find?q=" + SearchBox.Text + "&appid=" + Data.token + "&lang=ru");
                } catch (HttpRequestException) {
                    if (MessageBox.Show(Strings.get("INTERNET_ERROR_CONTENT", Data.language), Strings.get("INTERNET_ERROR_TITLE", Data.language), MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes) System.Diagnostics.Process.Start("https://malw.ru/pages/other#warp");
                    return;
                }
                var result = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
                if (result["cod"].ToString() != "200") {
                    if (result["message"].ToString().Equals("Nothing to geocode")) {
                        MessageBox.Show(Strings.get("STUPID_REQUEST", Data.language), Strings.get("ADDING_CITY", Data.language), MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    MessageBox.Show(result.ToString(), Strings.get("ADDING_CITY", Data.language), MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                
                if (result["count"].ToString() == "0") {
                    MessageBox.Show(Strings.get("NOTHING_FOUND", Data.language), Strings.get("ADDING_CITY", Data.language), MessageBoxButton.OK, MessageBoxImage.Warning);
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
