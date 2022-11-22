using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
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
        public async Task AddPetIDAsync(int petId)
        {
            petInfo.Id = petId;

            // Конвертирование данных пользователя в JSON для отправки на POST
            var json = JsonConvert.SerializeObject(petInfo);

            var response = await (await SendingRequestPostAsync("/pet", json)).Content.ReadAsStringAsync();

            //CheckActionResult("Питомец был успешно добавлен", "Добавление питомца завершилось ошибкой",
            //    (int)CheckInfo.FindPetInfoAsync(petId).Result.StatusCode, 200);
            //Console.WriteLine(response);
        }

        // Удаление существующего питомца
        public async Task DeletePetInfoAsync(int? petId, int httpCode = 404)
        {
            if (petId == null)
            {
               await SendingRequestDeleteAsync("/pet/");
            }
            else
            {
                await SendingRequestDeleteAsync("/pet/" + Convert.ToInt32(petId));
            }

            CheckActionResult("Успех!", $"Ошибка: Ожидалось:{httpCode}, Вернулся:{(int)CheckInfo.FindPetInfoAsync(petId).Result.StatusCode}",
                (int)CheckInfo.FindPetInfoAsync(petId).Result.StatusCode, httpCode);
        }

        // Обновление данных о питомце
        public async Task UpdatePetInfoAsync(int petId, int httpCode)
        {
            petInfo.Id = petId;
            
            // Конвертирование данных пользователя в JSON для отправки на POST
            var json  = (await SendingRequestPutAsync("/pet", JsonConvert.SerializeObject(petInfo))).Content.ReadAsStringAsync();
            var petBefore = await CheckInfo.FindPetInfoAsync(petId);
            var petAfter = await CheckInfo.FindPetInfoAsync(await json, petId);

            // Проверка что данные не были обновлены
            CheckActionResult("Успех!", $"Ошибка: Ожидалось:{httpCode}, Вернулся:{(int)petAfter.StatusCode}",
                (int)petAfter.StatusCode, httpCode);
        }

        // Найти данные о питомцах с определенным статусом
        public async Task FindByStatusAsync(string status)
        {

            var statusCode = (await SendingRequestGetAsync("/pet/findByStatus?status=" + status)).StatusCode;

            if (status =="sold" || status == "pending" || status == "available")
            {
                if ((int)statusCode == 200)
                {
                    Console.WriteLine("Питомец со статусом найден");
                }
                else
                {
                    throw new Exception($"Питомец со статусом {status} отсутсвует.\nКод статуса: {(int)statusCode}");
                }
            }
            else
            {
                if (statusCode == HttpStatusCode.OK )
                {
                    throw new Exception($"Питомец со статусом {status} отсутсвует.\nНеверный код статуса: {(int)statusCode}\nОжидалось: 404");
                }
                else
                {
                    throw new Exception($"Неизвестный статус:{status}\nОжидался один из:\n- sold\n- pending\n- available");
                }
            } 
        }       
    }
}
