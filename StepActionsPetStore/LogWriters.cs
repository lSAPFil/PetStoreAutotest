using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace SpecFlowProject_PetStore.StepActionsPetStore
{
    // Логирование результатов в файл
    public class LogWriter
    {
        private static string PathToLogFile = string.Empty;

        // Выводим данные логов теста в отдельный файл
        public static void LogWrite(string logMessage)
        {
            //Передаем путь до репозитория SpecFlowProject_PetStore
            PathToLogFile = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                // Добавляем результаты логирование в созданный файл TestRunResults.txt
                using (StreamWriter w = File.AppendText(PathToLogFile + "\\" + "TestRunResults.txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось добавить данные в файл");
            }
        }

        // Форматируем данные логов для их последующего добавления в файл
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
                throw new Exception("Ошибка на этапе логирования");
            }
        }
    }
}
