using System;
using System.ComponentModel;

namespace MelFitnessAssinaturas.Util
{
    public static class EnumHelpersUtil
    {

        public static string GetDescriptionValue(Enum value)
        {

            var fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();

        }

        public static object GetEnumValue(string value, Type enumType)
        {

            var names = Enum.GetNames(enumType);

            foreach (string name in names)
            {
                if (GetDescriptionValue((Enum)Enum.Parse(enumType, name)).Equals(value))
                {
                    return Enum.Parse(enumType, name);
                }
            }


            throw new ArgumentException("A string não é uma descrição ou um valor do enum especificado.");

        }

    }
}
