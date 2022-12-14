using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using NUnit.Framework;

// Класс для логирования результатов теста в файл
using static SpecFlowProject_PetStore.StepActionsPetStore.LogWriter;
using Newtonsoft.Json;

namespace SpecFlowProject_PetStore.StepActionsPetStore
{
    public class CheckInfo
    {
        // адрес сервиса PetStore
        public static string url = "https://petstore.swagger.io/v2";

        public static Random random = new Random();

        public static HttpWebResponse httpResponse;

        // Генерация случайных значений для отправки в сервис
        public static string RandomString(int length)
        {
            const string chars = "[]:;<>,./?|!@#$%^&*()_+=-"+
                "qwertyuiopasdfghjklzxcvbnm"+
                "QWERTYUIOPASDFGHJKLZXCVBNM"+
                "йцукёенгшщзхъфывапролджэячсмитьбю"+
                "ЙЦУКЕНГШЩЗХЪФЫВАПРОЛДЖЭЯЧСМИТЬБЮ"+
                "0123456789" +
                "  ~       ";

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // Вывод итога пройденного теста
        public static void CheckActionResult(string happyMessage, string badMessage, int? httpCodeType, int? waitingHttpCodeType)
        {
            // Проверка, что данные добавлены успешно
            if (httpCodeType == waitingHttpCodeType)
            {
                Console.WriteLine(happyMessage);
                LogWrite("Запуск сценария: "+ TestContext.CurrentContext.Test.Name.ToString() + "\nИтог шага: " + 
                    happyMessage + "\nКод статуса: " + httpCodeType);
            }
            else
            {
                throw new Exception($" {badMessage} {httpCodeType}");
                LogWrite("Запуск сценария: " + TestContext.CurrentContext.Test.Name.ToString() + "\nИтог шага: " + 
                    badMessage + "\nКод статуса: "+ httpCodeType);
            }
        }

        // Получение ответа сервера
        public static void GetHttpResponse(HttpWebRequest httpRequest)
        {
            // Избавляемся от остановки программы из-за негативного кода ответа
            try
            {
                httpResponse = httpRequest.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                httpResponse = ex.Response as HttpWebResponse;
            }
        }

        // Отправка запроса с пользовательскими параметрами
        public static int DeclareRequestSettings(string endPoint, string methodName)
        {
            Uri uri = new Uri(url + endPoint);
            // Объявление адреса для реквест запроса
            var httpRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpRequest.Method = methodName;

            // Получаем ответ от сервера
            GetHttpResponse(httpRequest);
            
            using var webStream = httpResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            Console.WriteLine(data);
            Console.WriteLine("Код статуса: " + (int)httpResponse.StatusCode);

            return (int)httpResponse.StatusCode;
        }

        // Отправка запроса с пользовательскими данными в json формате
        public static void DeclareRequestSettings(string endPoint, string methodName, string json)
        {
            Uri uri = new Uri(url + endPoint);
            // Объявление адреса для реквест запроса
            var httpRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpRequest.Method = methodName;

            // Задаем type данных для отправки
            httpRequest.ContentType = "application/json";

            // Получаем поток для отправки данных на сервер
            using (var requestStream = httpRequest.GetRequestStream())

            // Отправляем данные на сервер
            using (var writer = new StreamWriter(requestStream))
            {
                writer.Write(json);
            }

            // Получаем ответ от сервера
            GetHttpResponse(httpRequest);

            // Получаем поток для чтения ответа сервера
            using (var responseStream = httpResponse.GetResponseStream())

            // Читаем ответ сервера 
            using (var reader = new StreamReader(responseStream))
            {
                var response = reader.ReadToEnd();

                // Вывод ответа сервера
                Console.WriteLine(response);

                // Вывод полученного кода статуса
                Console.WriteLine("Код статуса: " + (int)httpResponse.StatusCode);
            }
        }

        // Получение информации о найденном питомце (код ответа)
        public static int FindPetInfoGetResponse(int? petId)
        {
            Uri uri = new Uri(url + "/pet/" + petId);
            // Объявление адреса для реквест запроса
            var httpRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpRequest.Method = "GET";

            // Получаем ответ от сервера
            GetHttpResponse(httpRequest);

            using var webStream = httpResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            return (int)httpResponse.StatusCode;
        }

        // Получение информации о найденном питомце
        public static string FindPetInfo(int? petId)
        {
            Uri uri = new Uri(url + "/pet/" + petId);
            // Объявление адреса для реквест запроса
            var httpRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpRequest.Method = "GET";

            // Получаем ответ от сервера
            GetHttpResponse(httpRequest);

            using var webStream = httpResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            return data;
        }

        // Найти информацию о питомце по ID
        public static int FindPetInfo(string json, int petId)
        {
            Uri uri = new Uri(url + "/pet/" + petId);
            // Объявление реквеста. Эндпоинт /user
            var httpRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpRequest.Method = "GET";

            // Получаем ответ от сервера
            GetHttpResponse(httpRequest);

            using var webStream = httpResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);

            // Новая информация по измененному питомцу
            var data = reader.ReadToEnd();

            // Сравнение как объекты (возможна перестановка переменных)

            if (JsonConvert.DeserializeObject(json) == JsonConvert.DeserializeObject(data))
            {
                throw new Exception("Данные о питомце не были обновлены");
            }

            return (int)httpResponse.StatusCode;
        }
    }
}
