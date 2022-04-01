using System;

namespace Jarvis.Utils.HelperClasses
{
    public static class ModuleUtils
    {
        public static int CalculatePercentage( int part , int total)
        {
            return ( int ) Math.Round( ( double ) (part * 100) / total );
        }
    }
}
