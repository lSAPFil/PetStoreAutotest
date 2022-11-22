using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using NUnit.Framework;

// Класс для логирования результатов теста в файл
using static SpecFlowProject_PetStore.StepActionsPetStore.LogWriter;
using Newtonsoft.Json;
using FluentAssertions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Collections.Generic;

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
        public void GetHttpResponse(HttpWebRequest httpRequest)
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

        // Отправка запроса с пользовательскими данными 
        public static async Task<HttpResponseMessage> SendingRequestGetAsync(string endPoint)
        {
            Uri uri = new Uri(url+endPoint);
            using var client = new HttpClient();

            // Задаем type данных для отправки
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var result = await client.GetAsync(uri);

            Console.WriteLine(result.StatusCode);

            return result;
        }

        public static async Task<HttpResponseMessage> SendingRequestPostAsync(string endPoint, string json)
        {
            Uri uri = new Uri(url + endPoint);
            using var client = new HttpClient();

            var result = await client.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));

            Console.WriteLine(result.StatusCode);

            return result;
        }

        public static async Task<HttpResponseMessage> SendingRequestPutAsync(string endPoint, string json)
        {
            using var client = new HttpClient();
            Uri uri = new Uri(url + endPoint);
            var result = await client.PutAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));

            Console.WriteLine(result.StatusCode);

            return result;
        }


        public static async Task<HttpResponseMessage> SendingRequestDeleteAsync(string endPoint)
        {
            using var client = new HttpClient();
            Uri uri = new Uri(url + endPoint);
            var result = await client.DeleteAsync(uri);

            Console.WriteLine(result.StatusCode);

            return result;
        }

        // Получение информации о найденном питомце
        public static async Task<HttpResponseMessage> FindPetInfoAsync(int? petId)
        {
            return await SendingRequestGetAsync("/pet/" + petId);
           
        }

        // Найти информацию о питомце по ID
        public static async Task<HttpResponseMessage> FindPetInfoAsync(string json, int petId)
        {
            // Получаем информацию по питомцу с сервисла
            var data = await (await SendingRequestGetAsync("/pet/" + petId)).Content.ReadAsStringAsync();

            // Сравнение как объекты (возможна перестановка переменных)

            // Новая отправленная информация по питомцу
            var expected = JsonConvert.DeserializeObject(json);

            // Полученная новая информация по питомцу
            var actual = JsonConvert.DeserializeObject(data);

            // Сравнение json по питомцу отправленного и полученного от сервиса
            actual.Should().BeEquivalentTo(expected, "Данные о питомце не были обновлены");

            return await SendingRequestGetAsync("/pet/" + petId);
        }
    }
}
