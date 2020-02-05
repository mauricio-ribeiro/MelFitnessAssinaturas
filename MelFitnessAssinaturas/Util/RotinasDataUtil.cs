using System;

namespace MelFitnessAssinaturas.Util
{
    public class RotinasDataUtil
    {
        public static bool CompareDatas(DateTime d1, DateTime d2)
        {
            var result = DateTime.Compare(d1, d2);
            return result <= 0;
        }

    }
}
