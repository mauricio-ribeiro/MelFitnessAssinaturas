using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MelFitnessAssinaturas.Util
{
    public static class StringUtil
    {

        public static bool CompareSecuryString(this SecureString ss1, SecureString ss2)
        {
            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;
            try
            {
                bstr1 = Marshal.SecureStringToBSTR(ss1);
                bstr2 = Marshal.SecureStringToBSTR(ss2);
                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2) return false;
                    }
                }
                else return false;
                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }
        }


        public static SecureString ConvertToSecureString(string valor)
        {

            if (valor == null)
                throw new ArgumentNullException($@"Não é permitido converter valores vazios !!");


            var secureValor = new SecureString();

            foreach (char item in valor)
                secureValor.AppendChar(item);

            return secureValor;

        }



    }
}
