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
        // Ввод данных о питомце для отправки запроса на сервис
        PetsInfo petInfo = new()
        {
            Id = 0,
            Category = new Category { Id = random.Next(10), Name = RandomString(random.Next(10)) },
            Name = RandomString(random.Next(10)),
            PhotoUrls = new List<string>() { RandomString(random.Next(10)) },
            Tags = new List<Category>() { new Category { Id = random.Next(10), Name = RandomString(random.Next(10)) } },
            Status = RandomString(random.Next(10))
        };

        // Добавление нового питомца
        public void AddPetID(int petId)
        {
            petInfo.Id = petId;

            // Конвертирование данных пользователя в JSON для отправки на POST
            var json = JsonConvert.SerializeObject(petInfo);

            DeclareRequestSettings("/pet", "POST", json);

            CheckActionResult("Питомец был успешно добавлен", "Добавление питомца завершилось ошибкой",
                CheckInfo.FindPetInfo(petId), 200);
        }

        // Удаление существующего питомца
        public void DeletePetInfo(int? petId)
        {
            if (petId == null)
            {
                DeclareRequestSettings("/pet/", "DELETE");
            }
            else
            {
                DeclareRequestSettings("/pet/" + Convert.ToInt32(petId), "DELETE");
            }

            CheckActionResult("Удаление прошло успешно", "Ошибка.\n Возможные причины:\nДанные о питомце не были удалены\nБыли неверно введены данные",
            CheckInfo.FindPetInfo(petId), 404);
        }

        // Обновление данных о питомце
        public void UpdatePetInfo(int petId)
        {
            petInfo.Id = petId;

            // Конвертирование данных пользователя в JSON для отправки на POST
            var json = JsonConvert.SerializeObject(petInfo);

            if (CheckInfo.FindPetInfo(petId) == 404)
            {
                DeclareRequestSettings("/pet", "PUT", json);

                // Проверка что данные не были обновлены
                CheckActionResult("Успех! Обновление несуществующего питомца невозможно", "Ошибка: Был обновлен несуществующий питомец",
                CheckInfo.FindPetInfo(petId), 404);
            }
            else
            {
                DeclareRequestSettings("/pet", "PUT", json);

                // Проверка, что данные обновлены успешно
                CheckActionResult("Обновление прошло успешно", "Ошибка: Обновить данные несуществующего питомца невозможно",
                CheckInfo.FindPetInfo(petId), 200);
            }
        }

        // Найти данные о питомцах с определенным статусом
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
