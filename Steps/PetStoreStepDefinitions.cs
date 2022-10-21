using FluentAssertions;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using SpecFlowProject_PetStore.StepActionsPetStore;
namespace SpecFlowProject_PetStore.Steps
{
    [Binding]
    public sealed class PetStoreStepDefinitions
    {
        public Pet pet = new();

        [Given(@"Добавить питомца с PetId (.*)")]
        public void StepAddPetID(int petId)
        {
            pet.AddPetID(petId);
        }

        [Given(@"Обновить данные питомца с PetId (.*)")]
        public void StepUpdatePetInfo(int petId)
        {
            pet.UpdatePetInfo(petId);
        }

        [Given(@"Удалить данные питомца с PetId (.*)")]
        public void StepDeletePetInfo(int petId)
        {
            pet.DeletePetInfo(petId);
        }

        [Given(@"Найти питомца по статусу (.*)")]
        public void StepFindByStatus(string status)
        {
            pet.FindByStatus(status);
        }
    }
}
