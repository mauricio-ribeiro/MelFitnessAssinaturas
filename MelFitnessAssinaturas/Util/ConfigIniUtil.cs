using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace MelFitnessAssinaturas.Util
{
    public static class ConfigIniUtil
    {

        private static string path = Application.StartupPath;
        private static string fileName = "Config.ini";

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        
        public static string Read(string section, string key)
        {
            var temp = new StringBuilder(255);
            var i = GetPrivateProfileString(section, key, "", temp, 255, path + "\\" + fileName);
            return temp.ToString();
        }
        
        public static void Write(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, path + "\\" + fileName);
        }
        

    }
}
