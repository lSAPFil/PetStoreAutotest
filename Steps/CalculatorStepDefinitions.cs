using FluentAssertions;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace SpecFlowProject_PetStore.Steps
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        
        class Calculator
        {
            // Объявляем список для хранения вводимых чисел 
            public List<int> numbersForAdd = new List<int>();
            public int result;

            public void Addition(List<int> numbers)
            {
                // Сложение введенных чисел
                foreach(int i in numbers)
                {
                    result += i;
                }
            }
        }

        private readonly Calculator _calculator = new Calculator();

        [Given(@"Число для операции сложения равно (.*)")]
        public void GivenIHaveEnteredIntoTheCalculator(int number)
        {
            // Заполнение списка вводимыми числами
            _calculator.numbersForAdd.Add(number);
        }

        [When(@"Сложить все числа")]
        public void WhenIPressAdd()
        {
            // Сложение вводимых чисел
            _calculator.Addition(_calculator.numbersForAdd);
        }

    }
}
