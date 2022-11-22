using FluentAssertions;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using SpecFlowProject_PetStore.StepActionsPetStore;
using static SpecFlowProject_PetStore.StepActionsPetStore.CheckInfo;
using static SpecFlowProject_PetStore.StepActionsPetStore.StepFunction;
namespace SpecFlowProject_PetStore.Steps
{
    [Binding]
    public sealed class PetStoreStepDefinitions
    {
        public StepFunction pet = new();

        [Given(@"Добавить питомца с PetId (.*)")]
        public void StepAddPetID(int petId)
        {
            pet.AddPetIDAsync(petId).Wait();
        } 
        
        [Given(@"Добавить питомца ")]
        public void StepAddPetIDAndHeaders(int petId)
        {
            pet.AddPetIDAsync(petId).Wait();
        }

        [Given(@"Найти питомца по PetId (.*) и создать нового в случае неудачи")]
        public void StepFindPetById(int petId)
        {
            if ((int)CheckInfo.FindPetInfoAsync(petId).Result.StatusCode != 200)
            {
                StepAddPetID(petId);
            }
            else
            {
                Console.WriteLine($"Питомец с PetId {petId} существует!");
            }
        }

        [Given(@"Обновить данные питомца с PetId ""(.*)"" ожидаемый результат ""(.*)""")]
        public void StepUpdatePetInfo(int petId, int httpCode=200)
        {
            pet.UpdatePetInfoAsync(petId, httpCode).Wait();
        }

        [Given(@"Удалить данные питомца без PetId")]
        public void StepDeleteWithoutID()
        {
            pet.DeletePetInfoAsync(null, 405).Wait();
        }

        [Given(@"Удалить данные питомца с PetId (.*)")]
        public void StepDeletePetInfo(int petId)
        {
            pet.DeletePetInfoAsync(petId).Wait();
        }

        [Given(@"Найти питомца по статусу ""(.*)""")]
        public void StepFindByStatus(string status)
        {
            pet.FindByStatusAsync(status).Wait();
        }

        [Given(@"Найти питомца по id (.*)")]
        public void StepFindById(int id)
        {

            if ((int)CheckInfo.FindPetInfoAsync(id).Result.StatusCode == 200)
            {
                throw new Exception("Данный питомец существует!");
            }
            else
            {
                Console.WriteLine($"Несуществующий питомец не был найден\n Код статуса: { (int)CheckInfo.FindPetInfoAsync(id).Result.StatusCode}");
            }
        }
    }
}
