using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpecFlowProject_PetStore
{
    [Binding]
    class Hooks
    {
        // before
        // after
        // start step
        // finish step
        [Before]
        public static void something()
        {
            Console.WriteLine("Before scenario");
        }

        [After]
        public void ctoto()
        {
            Console.WriteLine("After scenario");
        }
    }
}
