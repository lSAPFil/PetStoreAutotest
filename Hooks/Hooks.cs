using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using SpecFlowProject_PetStore.Steps;
using NUnit.Framework;

namespace SpecFlowProject_PetStore
{
    [Binding]
    class Hooks
    {
        public CalculatorStepDefinitions.Calculator calculator = new();
        // before
        // after
        // start step
        // finish step
        [Before]
        public static void BeforeScenario()
        {
            Console.WriteLine("Запуск сценария: " + TestContext.CurrentContext.Test.Name);
        }

        [After]
        public static void AfterScenario()
        {
            Console.WriteLine("Тест завершился "+ DateTime.Now.ToString());
        }
    }
}
