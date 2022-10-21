using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using SpecFlowProject_PetStore;

namespace SpecFlowProject_PetStore.StepActionsPetStore
{
    public sealed class Pet
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "[]:;<>,./?|!@#$%^&*()_+=-qqwertyuiopasdfghjklzxcvbnmйцукенгшщзхъфывапролджэячсмитьбю0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string url = "https://petstore.swagger.io/v2";

        public HttpWebResponse httpResponse;

        PetsInfo petInfo = new()
        {
            Id = 2,
            Category = new Category { Id = random.Next(10), Name = RandomString(random.Next(10)) },
            Name = RandomString(random.Next(10)),
            PhotoUrls = new List<string>() { RandomString(random.Next(10)) },
            Tags = new List<Category>() { new Category { Id = random.Next(10), Name = RandomString(random.Next(10)) } },
            Status = RandomString(random.Next(10))
        };

        public void CheckActionResult(string happyMessage, string badMessage, int httpCodeType, int waitingHttpCodeType)
        {
            // Проверка, что данные добавлены успешно
            if (httpCodeType == waitingHttpCodeType)
            {
                Console.WriteLine(happyMessage);
            }
            else
            {
                throw new Exception($" {badMessage} {httpCodeType}");
            }
        }

        public void AddPetID(int petId)
        {
            petInfo.Id = petId;

            // Конвертирование данных пользователя в JSON для отправки на POST
            var json = JsonConvert.SerializeObject(petInfo);

            DeclareRequestSettings("/pet", "POST", json);

            CheckActionResult("Питомец был успешно добавлен", "Добавление питомца завершилось ошибкой", FindPetInfo(Convert.ToInt32(petInfo.Id)), 200);
        }

        public void DeletePetInfo(int petId)
        {

            DeclareRequestSettings("/pet/" + petId, "DELETE");

            CheckActionResult("Удаление прошло успешно", "Данные о питомце не были удалены", FindPetInfo(petId), 404);
        }

        public void GetHttpResponse(HttpWebRequest httpRequest)
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

        public void DeclareRequestSettings(string endPoint, string methodName)
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
        }

        public int FindPetInfo(int petId)
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

        public int FindPetInfo(string json, int petId)
        {
            Uri uri = new Uri(url + "/pet/" + petId);
            // Объявление реквеста. Эндпоинт /user
            var httpRequest = (HttpWebRequest)WebRequest.Create(uri);

            httpRequest.Method = "GET";

            // Получаем ответ от сервера
            GetHttpResponse(httpRequest);

            using var webStream = httpResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            if (json == data)
            {
                Console.WriteLine("Обновление прошло успешно!");
            }
            else
            {
                throw new Exception("Данные о питомце не были обновлены");
            }

            return (int)httpResponse.StatusCode;
        }

        public void DeclareRequestSettings(string endPoint, string methodName, string json)
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

        public void UpdatePetInfo(int petId)
        {
            petInfo.Id = petId;

            // Конвертирование данных пользователя в JSON для отправки на POST
            var json = JsonConvert.SerializeObject(petInfo);

            DeclareRequestSettings("/pet", "PUT", json);

            // Проверка, что данные обновлены успешно
            FindPetInfo(json, petId);
        }

        public void FindByStatus(string status)
        {
            // Объявление реквеста
            DeclareRequestSettings("/pet/findByStatus?status=" + status, "GET");
        }
    }
}
