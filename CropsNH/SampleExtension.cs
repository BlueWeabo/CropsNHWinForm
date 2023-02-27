using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CropsNH
{
    // Test:

    //string s1 = "abcdef";
    //string s2 = s1.ExampleReverse();

    //int m = 10;
    //bool b = m.ExampleIsBigger(13);


    // 1. pudlic static
    public static class SampleExtensions
    {
        // 2. public static 
        // 3. (this extension_type T [, parameters])
        public static string ExampleReverse(this string inputString)
        {
            string resultString = string.Empty;

            if (!string.IsNullOrEmpty(inputString))
            {
                foreach (char c in inputString)
                {
                    resultString = c + resultString;
                }
            }

            return resultString;
        }

        public static bool ExampleIsBigger(this int baseValue, int x)
        {
            return (baseValue > x);
        }
    }
}
