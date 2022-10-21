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

        PetsInfo petInfo = new()
        {
            Id = 2,
            Category = new Category { Id = random.Next(10), Name = RandomString(random.Next(10)) },
            Name = RandomString(random.Next(10)),
            PhotoUrls = new List<string>() { RandomString(random.Next(10)) },
            Tags = new List<Category>() { new Category { Id = random.Next(10), Name = RandomString(random.Next(10)) } },
            Status = RandomString(random.Next(10))
        };

        public void AddPetID(int petId)
        {
            petInfo.Id = petId;

            // Объявление адреса для отправки данных
            var httpRequest = (HttpWebRequest)WebRequest.Create(url + "/pet");

            httpRequest.Method = "POST";

            // Конвертирование данных пользователя в JSON для отправки на POST
            var json = JsonConvert.SerializeObject(petInfo);

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
            HttpWebResponse httpResponse;
            httpResponse = httpRequest.GetResponse() as HttpWebResponse;
            
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

            // Проверка, что данные добавлены успешно
            if (FindPetInfo(Convert.ToInt32(petInfo.Id)) == 200)
            {
                Console.WriteLine("Питомец был успешно добавлен");
            }
            else
            {
                throw new Exception($"Добавление питомца завершилось ошибкой {FindPetInfo(Convert.ToInt32(petInfo.Id))}");
            }
        }

        public void DeletePetInfo(int petId)
        {
            // Объявление адреса для отправки данных
            var httpRequest = (HttpWebRequest)WebRequest.Create(url + "/pet/" + petId);

            httpRequest.Method = "DELETE";

            // Получаем ответ от сервера
            HttpWebResponse httpResponse;
            httpResponse = httpRequest.GetResponse() as HttpWebResponse;

            using var webStream = httpResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            Console.WriteLine("Код статуса: " + (int)httpResponse.StatusCode);

            // Проверка, что данные удалены успешно
            if (FindPetInfo(petId) == 404)
            {
                Console.WriteLine("Удаление прошло успешно");
            }
            else
            {
                throw new Exception("Данные о питомце не были удалены");
            }
        }

        public int FindPetInfo(int petId)
        {
            // Объявление адреса для отправки данных
            var httpRequest = (HttpWebRequest)WebRequest.Create(url + "/pet/" + petId);

            httpRequest.Method = "GET";

            // Получаем ответ от сервера
            HttpWebResponse httpResponse;
            try
            {
                httpResponse = httpRequest.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                httpResponse = ex.Response as HttpWebResponse;
            }

            using var webStream = httpResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            return (int)httpResponse.StatusCode;
        }

        public int FindPetInfo(string json, int petId)
        {
            // Объявление реквеста. Эндпоинт /user
            var httpRequest = (HttpWebRequest)WebRequest.Create(url + "/pet/" + petId);

            httpRequest.Method = "GET";

            // Получаем ответ от сервера
            HttpWebResponse httpResponse;
            httpResponse = httpRequest.GetResponse() as HttpWebResponse;
            
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

        public void UpdatePetInfo(int petId)
        {
            petInfo = new()
            {
                Id = petId,
                Category = new Category { Id = random.Next(10), Name = RandomString(random.Next(10)) },
                Name = RandomString(random.Next(10)),
                PhotoUrls = new List<string>() { RandomString(random.Next(10)) },
                Tags = new List<Category>() { new Category { Id = random.Next(10), Name = RandomString(random.Next(10)) } },
                Status = RandomString(random.Next(10))
            };

            // Объявление реквеста. Эндпоинт /user
            var httpRequest = (HttpWebRequest)WebRequest.Create(url + "/pet");

            httpRequest.Method = "PUT";

            // Конвертирование данных пользователя в JSON для отправки на POST
            var json = JsonConvert.SerializeObject(petInfo);

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
            HttpWebResponse httpResponse;
            httpResponse = httpRequest.GetResponse() as HttpWebResponse;
           
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

            // Проверка, что данные обновлены успешно
            FindPetInfo(json, petId);
        }

        public void FindByStatus(string status)
        {
            // Объявление реквеста. Эндпоинт /user
            var httpRequest = (HttpWebRequest)WebRequest.Create(url + "/pet/findByStatus?status=" + status);

            httpRequest.Method = "GET";

            // Получаем ответ от сервера
            HttpWebResponse httpResponse;
            httpResponse = httpRequest.GetResponse() as HttpWebResponse;
            
            using var webStream = httpResponse.GetResponseStream();

            using var reader = new StreamReader(webStream);
            var data = reader.ReadToEnd();

            Console.WriteLine("Код статуса: " + (int)httpResponse.StatusCode);
        }
    }
}
