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
            pet.AddPetID(petId);
        } 
        
        [Given(@"Добавить питомца ")]
        public void StepAddPetIDAndHeaders(int petId)
        {
            pet.AddPetID(petId);
        }

        [Given(@"Найти питомца по PetId (.*) и создать нового в случае неудачи")]
        public void StepFindPetById(int petId)
        {
            if (FindPetInfoGetResponse(petId) != 200)
            {
                StepAddPetID(petId);
            }
            else
            {
                Console.WriteLine($"Питомец с PetId {petId} существует!");
            }
        }

        [Given(@"Обновить данные питомца с PetId (.*)")]
        public void StepUpdatePetInfo(int petId)
        {
            pet.UpdatePetInfo(petId);
        }

        [Given(@"Удалить данные питомца без PetId")]
        public void StepDeleteWithoutID()
        {
            pet.DeletePetInfo(null);
        }

        [Given(@"Удалить данные питомца с PetId (.*)")]
        public void StepDeletePetInfo(int petId)
        {
            pet.DeletePetInfo(petId);
        }

        [Given(@"Найти питомца по статусу ""(.*)""")]
        public void StepFindByStatus(string status)
        {
            pet.FindByStatus(status);
        }

        [Given(@"Найти питомца по id (.*)")]
        public void StepFindById(int id)
        {

            if (FindPetInfoGetResponse(id) == 200)
            {
                throw new Exception("Данный питомец существует!");
            }
            else
            {
                Console.WriteLine($"Несуществующий питомец не был найден\n Код статуса: {FindPetInfoGetResponse(id)}");
            }
        }
    }
}
