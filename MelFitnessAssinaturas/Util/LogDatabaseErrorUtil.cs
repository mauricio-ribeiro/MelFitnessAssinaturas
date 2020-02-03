using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace MelFitnessAssinaturas.Util
{
    public class LogDatabaseErrorUtil
    {

        public static string CreateErrorDatabaseMessage(SqlException serviceException)
        {

            var messageBuilder = new StringBuilder();

            try
            {

                messageBuilder.Append("Exception: " + serviceException);

                if (serviceException.InnerException != null)
                {

                    messageBuilder.Append("InnerException: " + serviceException.InnerException);

                }

                return messageBuilder.ToString();
            }
            catch
            {
                messageBuilder.Append("Exception: Exceção Desconhecida.");
                return messageBuilder.ToString();
            }

        }


        public static string ValidateDataBaseErrorNumber(int number)
        {

            var msg = string.Empty;

            switch (number)
            {
                case 102: msg = "Erro de sintaxe no SQL."; break;
                case 207: msg = "Coluna desconhecida na tabela."; break;
                case 208: msg = "Tabela não existe no banco de dados."; break;
                case 8152: msg = "Dado muito longo para a coluna."; break;
                default: msg = "Ocorreu um erro no Banco de Dados."; break;
            }

            return msg;
        }

        public static string CreateErrorMessage(Exception serviceException)
        {
            var messageBuilder = new StringBuilder();

            try
            {

                messageBuilder.Append("Exception: " + serviceException);

                if (serviceException.InnerException != null)
                {
                    messageBuilder.Append("InnerException: " + serviceException.InnerException);
                }

                return messageBuilder.ToString();
            }
            catch
            {
                messageBuilder.Append("Exception: Unknown Exception.");
                return messageBuilder.ToString();
            }

        }

        public static void LogFileWrite(string message, string strFunctionName)
        {

            FileStream fileStream = null;
            StreamWriter streamWriter = null;

            try
            {
                string logFilePath = AppDomain.CurrentDomain.BaseDirectory + "LogError\\";

                var data = DateTime.Now;
                var strData = data.ToString("dd-MM-yyyy");
                var strHora = data.ToShortTimeString();

                strHora = strHora.Replace(":", "").Replace(":", "").Replace(":", "");

                var dataEhora = strData + "_" + strHora;

                logFilePath = logFilePath + strFunctionName + "-" + strData + "_" + strHora + ".txt";

                if (logFilePath.Equals("")) return;

                DirectoryInfo logDirInfo = null;
                var logFileInfo = new FileInfo(logFilePath);
                logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);


                if (!logFileInfo.Exists)
                {
                    fileStream = logFileInfo.Create();
                }
                else
                {
                    fileStream = new FileStream(logFilePath, FileMode.Append);
                }

                streamWriter = new StreamWriter(fileStream);
                streamWriter.WriteLine(message);
            }
            finally
            {
                streamWriter?.Close();
                fileStream?.Close();

            }


        }




    }
}
