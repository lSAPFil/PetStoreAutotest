using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using static SpecFlowProject_PetStore.StepActionsPetStore.LogWriter;
using NUnit.Framework;

namespace SpecFlowProject_PetStore.StepActionsPetStore
{
    public class CheckInfo
    {
        public static string url = "https://petstore.swagger.io/v2";

        public static Random random = new Random();

        public static HttpWebResponse httpResponse;

        //public LogWriter log = new LogWriter;

        public static string RandomString(int length)
        {
            const string chars = "[]:;<>,./?|!@#$%^&*()_+=-qqwertyuiopasdfghjklzxcvbnmйцукенгшщзхъфывапролджэячсмитьбю0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void CheckActionResult(string happyMessage, string badMessage, int httpCodeType, int waitingHttpCodeType)
        {
            // Проверка, что данные добавлены успешно
            if (httpCodeType == waitingHttpCodeType)
            {
                Console.WriteLine(happyMessage);
                LogWrite("Запуск сценария: "+ TestContext.CurrentContext.Test.Name.ToString() + "\nИтог шага: " + happyMessage + "\nКод статуса: " + httpCodeType);
            }
            else
            {
                throw new Exception($" {badMessage} {httpCodeType}");
                LogWrite("Запуск сценария: " + TestContext.CurrentContext.Test.Name.ToString() + "\nИтог шага: " + badMessage + "\nКод статуса: "+ httpCodeType);
            }
        }

        public static void GetHttpResponse(HttpWebRequest httpRequest)
        {
            try
            {
                httpResponse = httpRequest.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                httpResponse = ex.Response as HttpWebResponse;
            }
        }

        public static int DeclareRequestSettings(string endPoint, string methodName)
        {
            Uri uri = new Uri(url + endPoint);
            var httpRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpRequest.Method = methodName;

            GetHttpResponse(httpRequest);

            // Получаем ответ от сервера
            using var webStream = httpResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            Console.WriteLine("Код статуса: " + (int)httpResponse.StatusCode);

            return (int)httpResponse.StatusCode;
        }

        public static int FindPetInfo(int petId)
        {
            Uri uri = new Uri(url + "/pet/" + petId);
            // Объявление адреса для отправки данных
            var httpRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpRequest.Method = "GET";

            // Получаем ответ от сервера
            GetHttpResponse(httpRequest);

            using var webStream = httpResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            return (int)httpResponse.StatusCode;
        }

        public static void DeclareRequestSettings(string endPoint, string methodName, string json)
        {
            Uri uri = new Uri(url + endPoint);
            // Объявление реквеста. Эндпоинт
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
    }
}
