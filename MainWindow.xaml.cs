﻿using System.Windows;
using System;
using System.IO;
using System.Data.SQLite;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using System.Collections.Generic;

namespace Weather_Informer
{
    public static class Data
    {
        public static readonly string token = "2a605935cb485e63d8657f2f7c2774e9";
        public static bool UseFahrenheit;
        public static int CityID;
        public static string CityFriendlyName;
        public static string language;
        public static bool tray;
    }

    public static class Database {
        public static readonly string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Malw_weather_informer.db");
        private static SQLiteConnection connection;

        public static void Create() {
            SQLiteConnection.CreateFile(path);
            Init();
            ExecAndLeave("CREATE TABLE IF NOT EXISTS Cities (id INTEGER UNIQUE PRIMARY KEY, FriendlyName VARCHAR)",
                "CREATE TABLE IF NOT EXISTS Settings (UseFahrenheit BOOLEAN, Language VARCHAR(2), CurrentCity INTEGER, NotificationsEnabled BOOLEAN, FOREIGN KEY (CurrentCity) REFERENCES Cities(id))");
        }

        public static void Init() {
            connection = new SQLiteConnection($"Data Source={path};Version=3;");
        }

        public static void ExecAndLeave(params string[] commands) {
            connection.Open();
            foreach (string command in commands) {
                try {
                    new SQLiteCommand(command, connection).ExecuteNonQuery();
                } catch (SQLiteException e) {
                    if (!e.Message.Contains("UNIQUE constraint failed")) {
                        MessageBox.Show(e.Message, "Ошибка базы данных!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            connection.Close();
        }

        public static string ExecAndReturn(string command, string column) {
            string result = null;
            connection.Open();
            SQLiteDataReader reader = new SQLiteCommand(command, connection).ExecuteReader();
            if (reader.Read()) {
                result = reader[column].ToString();
            }
            connection.Close();
            return result;
        }

        public static Dictionary<int, string> GetCities() {
            Dictionary<int, string> cities = new Dictionary<int, string>();
            connection.Open();
            SQLiteDataReader reader = new SQLiteCommand("SELECT * FROM Cities", connection).ExecuteReader();
            while (reader.Read()) {
                cities.Add(Convert.ToInt32(reader[0]), reader[1].ToString());
            }
            connection.Close();
            return cities;
        }

        public static string[] GetSettings() {
            string[] settings = new string[4];
            connection.Open();
            SQLiteDataReader reader = new SQLiteCommand("SELECT * FROM Settings", connection).ExecuteReader();
            if (reader.Read()) {
                settings = new string[4] {reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString()};
            }
            connection.Close();
            return settings;
        }
    }

    public partial class MainWindow : Window
    {
        public MainWindow() {
            if (!File.Exists(Database.path)) {
                new AddCity().ShowDialog();
                if (AddCity.SelectedCityId == 0) Environment.Exit(1);
                Database.Create();
                Database.ExecAndLeave("INSERT INTO Cities VALUES (" + AddCity.SelectedCityId.ToString() + ", \"" + AddCity.SelectedCityName + "\")",
                    "INSERT INTO Settings VALUES (0, \"ru\", " + AddCity.SelectedCityId.ToString() + ", 0)");
                Data.UseFahrenheit = false;
                Data.language = "ru";
                Data.CityID = AddCity.SelectedCityId;
                Data.tray = false;
                Data.CityFriendlyName = AddCity.SelectedCityName;
            }
            else {
                Database.Init();
                string[] settings = Database.GetSettings();
                Data.UseFahrenheit = Convert.ToBoolean(settings[0]);
                Data.language = settings[1];
                Data.CityID = Convert.ToInt32(settings[2]);
                Data.tray = Convert.ToBoolean(settings[3]);
                Data.CityFriendlyName = Database.ExecAndReturn("SELECT FriendlyName FROM Cities WHERE id = " + Data.CityID.ToString(), "FriendlyName");
            }
            InitializeComponent();
            city_name.Text = Data.CityFriendlyName;
            TheWindow.Title = "Weather Informer — В городе " + Data.CityFriendlyName + " обновление информации...";
            Refresh();
        }

        async void Refresh() {
            RefreshButton.IsEnabled = false;
            current_temperature.Text = "...";
            current_description.Text = "Получение информации...";
            TheWindow.Title = "Weather Informer — В городе " + Data.CityFriendlyName + " обновление информации...";
            city_name.Text = Data.CityFriendlyName;
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync("https://api.openweathermap.org/data/2.5/forecast?id=" + Data.CityID.ToString() + "&appid=" + Data.token + "&lang="+Data.language+"&units=" + (Data.UseFahrenheit ? "imperial" : "metric"));
                var result = JsonConvert.DeserializeObject<JObject>(await response.Content.ReadAsStringAsync());
                if (Data.CityFriendlyName != result["city"]["name"].ToString()) {
                    Data.CityFriendlyName = result["city"]["name"].ToString();
                    city_name.Text = Data.CityFriendlyName;
                    Database.ExecAndLeave("UPDATE Cities SET FriendlyName=\"" + Data.CityFriendlyName + "\" WHERE id=" + Data.CityID.ToString());
                }

                string units = Data.UseFahrenheit ? "F" : "C";
                current_temperature.Text = Convert.ToInt32(result["list"][0]["main"]["temp"]).ToString() + "°" + units;
                current_description.Text = result["list"][0]["weather"][0]["description"].ToString() + ". Ощущается как " + Convert.ToInt32(result["list"][0]["main"]["feels_like"]).ToString() + "°" + units;
                current_description.Text = char.ToUpper(current_description.Text[0]) + current_description.Text.Substring(1);
                TheWindow.Icon = new BitmapImage(new Uri("pack://application:,,,/res/" + result["list"][0]["weather"][0]["icon"].ToString().Substring(0, 2) + ".ico"));
                current_icon.Source = new BitmapImage(new Uri("pack://application:,,,/res/" + result["list"][0]["weather"][0]["icon"].ToString().Substring(0, 2) + ".png"));
                TheWindow.Title = "Weather Informer — В городе " + Data.CityFriendlyName + " " + result["list"][0]["weather"][0]["description"].ToString() + ", " + current_temperature.Text + " (" + Convert.ToInt32(result["list"][0]["main"]["feels_like"]).ToString() + "°" + units + ")";
                TheWindow.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/res/" + result["list"][0]["weather"][0]["icon"].ToString().Substring(0, 2) + ".jpg")));

                JArray Forecast = (JArray)result["list"];
                JArray NotTodayFC = new JArray();
                foreach (JToken e in Forecast) if (DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(e["dt"])).DateTime.Date != DateTime.Today) NotTodayFC.Add(e);

                foreach (int d in new int[] {8, 16, 24, 32, 40})
                {
                    TextBlock t = (TextBlock)FindName("d" + d.ToString());
                    Grid g = (Grid)FindName("c" + d.ToString());
                    try { 
                        t.Text = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt32(NotTodayFC[d]["dt"])).Date.ToString("dddd, d MMMM");
                        t.Visibility = Visibility.Visible;
                        g.Visibility = Visibility.Visible;
                    }
                    catch { }
                }

                for (int e = 0; e <= 40; e++) {
                    TextBlock t = (TextBlock)FindName("t"+e.ToString());
                    Image i = (Image)FindName("i" + e.ToString());
                    try {
                        t.Text = NotTodayFC[e]["main"]["temp"].ToString() + "°" + units;
                        i.Source = new BitmapImage(new Uri("pack://application:,,,/res/" + NotTodayFC[e]["weather"][0]["icon"].ToString().Substring(0, 2) + ".png"));
                        i.ToolTip = NotTodayFC[e]["weather"][0]["description"].ToString();
                        i.ToolTip = char.ToUpper(i.ToolTip.ToString()[0]) + i.ToolTip.ToString().Substring(1);
                        ToolTipService.SetInitialShowDelay(i, 0);
                    }
                    catch {
                        t.Text = "???";
                        i.Source = new BitmapImage(new Uri("pack://application:,,,/res/unknown.png"));
                        i.ToolTip = "Неизвестно";
                    }
                }
            }
            RefreshButton.IsEnabled = true;
        }

        private void Refresh(object sender, RoutedEventArgs e) { Refresh(); }

        private void OpenSettings(object sender, RoutedEventArgs e) {
            new Settings().ShowDialog();
        }

        private void CityManager(object sender, RoutedEventArgs e) {
            new CityManager().ShowDialog();
            Refresh();
        }
        // 3) Создать фигнюшку для получения уведов


        // Настройки:
        // 1) Цельсий/фаренгейт
        // 2) Язык - если будет не лень
        // 3) Запуск при старте
        // 4) Вкл/выкл уведы
    }
}
