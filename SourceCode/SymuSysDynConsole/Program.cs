using System;
using Symu.SysDyn;

namespace SymuSysDynConsole
{
    class Program
    {
        static void Main(string[] args)
        {            //string location of the System Dynamics Model file
            string xml = @"C:\Users\laure\Dropbox\Symu\SourceCode\Symu.SysDyn\Github\SourceCode\SymuSysDyn\Templates\Fishmodel.stmx";

            //Model object using location
            StateMachine testModel = new StateMachine(xml, false);

            //run 30 timesteps and print the number of fish at each time step.
            for (int counter = 0; counter < 30; counter++)
            {
                //Programmers need to know key terms from the model such as "Fish" to access the stock variable 'Fish' in the model.
                //These can be found in the .stmx model file
                Console.WriteLine("Fish = " + testModel.GetVariable("Fish"));
                //Console.WriteLine("Difference = " + testModel.getDifference("Fish"));
                testModel.Process();
            }
        }

    }
}
