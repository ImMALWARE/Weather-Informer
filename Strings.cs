using System.Collections.Generic;

namespace Weather_Informer
{
    internal static class Strings {

        private static readonly Dictionary<string, string> ru = new Dictionary<string, string> {
            {"DB_ERROR", "Ошибка базы данных!"},
            {"TITLE", "Weather Informer — В городе %city% %info%"},
            {"REFRESHING", "обновление информации..."},
            {"GETTING", "Получение информации..."},
            {"UNKNOWN", "Неизвестно"},
            {"FEELS_LIKE", ". Ощущается как "},
            {"STUPID_REQUEST", "Введите нормальный запрос!" },
            {"ADDING_CITY", "Добавление города"},
            {"NOTHING_FOUND", "Не найдены города по этому запросу!"},
            {"NOTIFICATION_TITLE", "В городе %city% %temp%"},
            {"INTERNET_ERROR_TITLE", "Ошибка отправки запроса!"},
            {"INTERNET_ERROR_CONTENT", "Попробуйте сменить IP адрес (перезагрузить роутер или использовать VPN). Показать рекомендуемый VPN-сервис?"},
            {"ERROR_TITLE", "Weather Informer — Ошибка"}
        };

        private static readonly Dictionary<string, string> en = new Dictionary<string, string> {
            {"DB_ERROR", "Database error!"},
            {"TITLE", "Weather Informer — It's %info% in %city%"},
            {"REFRESHING", "refreshing"},
            {"GETTING", "Getting info..."},
            {"UNKNOWN", "Unknown"},
            {"FEELS_LIKE", ". Feels like "},
            {"STUPID_REQUEST", "Enter adequate request!"},
            {"ADDING_CITY", "Adding city"},
            {"NOTHING_FOUND", "No cities found!"},
            {"NOTIFICATION_TITLE", "It's %temp% in %city%"},
            {"INTERNET_ERROR_TITLE", "Couldn't send request!"},
            {"INTERNET_ERROR_CONTENT", "Try to change IP address (reboot router or use VPN). Show recommended VPN service?"},
            {"ERROR_TITLE", "Weather Informer — Error"}
        };

        public static string get(string id, string lang) {
            if (lang.Equals("ru")) return ru[id];
            else return en[id];
        }


    }
}
