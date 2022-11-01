using FluentAssertions;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
//библиотека для сравнения json


namespace SpecFlowProject_PetStore.Steps
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        public class Calculator
        {
            // Объявляем список для хранения вводимых чисел 
            public List<int> numbersForAdd = new();
            public int result;
            public void Addition(List<int> numbers)
            {
                // Сложение введенных чисел
                foreach (int i in numbers)
                {
                    result += i;
                }

                Console.WriteLine($"Ответ: {result}");
            }
        }

        public Calculator _calculator = new();

        [Given(@"Число для операции сложения равно (.*)")]
        public void GivenIHaveEnteredIntoTheCalculator(int number)
        {
            // Заполнение списка вводимыми числами
            _calculator.numbersForAdd.Add(number);
        }

        [Given(@"Сложить все числа")]
        public void WhenIPressAdd()
        {
            // Сложение вводимых чисел
            _calculator.Addition(_calculator.numbersForAdd);
        }
    }
}
