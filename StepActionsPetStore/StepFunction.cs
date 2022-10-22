using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

using static SpecFlowProject_PetStore.StepActionsPetStore.CheckInfo;

namespace SpecFlowProject_PetStore.StepActionsPetStore
{
    public sealed class StepFunction
    {
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

            // Конвертирование данных пользователя в JSON для отправки на POST
            var json = JsonConvert.SerializeObject(petInfo);

            DeclareRequestSettings("/pet", "POST", json);

            CheckActionResult("Питомец был успешно добавлен", "Добавление питомца завершилось ошибкой", CheckInfo.FindPetInfo(Convert.ToInt32(petInfo.Id)), 200);
        }

        public void DeletePetInfo(int petId)
        {

            DeclareRequestSettings("/pet/" + petId, "DELETE");

            CheckActionResult("Удаление прошло успешно", "Данные о питомце не были удалены", CheckInfo.FindPetInfo(petId), 404);
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
            var statusCode = DeclareRequestSettings("/pet/findByStatus?status=" + status, "GET");

            if (status =="sold" || status == "pending" || status == "available")
            {
                if (statusCode == 200)
                {
                    Console.WriteLine("Питомец со статусом найден");
                }
                else
                {
                    throw new Exception($"Питомец со статусом {status} отсутсвует.\nКод статуса: {statusCode}");
                }
            }
            else
            {
                if (statusCode == 200)
                {
                    throw new Exception($"Питомец со статусом {status} отсутсвует.\nНеверный код статуса: {statusCode}\nОжидалось: 404");
                }
                else
                {
                    throw new Exception($"Неизвестный статус:{status}\nОжидался один из:\n- sold\n= pending\n- available");
                }
            } 
        }
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
    }
}
