using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MelFitnessAssinaturas.Util
{
    public class ConfigIniUtil
    {

        private static string filePath = Application.StartupPath + "\\Config.ini";


        [DllImport("kernel32", EntryPoint = "WritePrivateProfileString")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32", EntryPoint = "GetPrivateProfileString")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        public static string Read(string section, string key)
        {
            var temp = new StringBuilder(255);
            var i = GetPrivateProfileString(section, key, "", temp, 255, filePath);
            return temp.ToString();
        }
        
        public static void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, filePath);
        }
        

    }
}
