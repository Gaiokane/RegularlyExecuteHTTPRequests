using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegularlyExecuteHTTPRequests
{
    public class PublicVariables
    {
        public static int exectimescron = 0;

        public static void execTimesCronAddOne()
        {
            exectimescron++;
        }

        public static void execTimesCronReset()
        {
            exectimescron = 0;
        }
    }
}
