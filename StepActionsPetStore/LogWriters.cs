using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace SpecFlowProject_PetStore.StepActionsPetStore
{
    public class LogWriter
    {
        private static string m_exePath = string.Empty;
        public static void LogWrite(string logMessage)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "TestRunResults.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public static void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("---------------------------------------------------------");
                txtWriter.Write("\r\nНачало запуска: ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("   |\n   |{0}", logMessage);
                txtWriter.WriteLine("---------------------------------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
