using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UeiAdapter
{
    class Converters
    {
        public static double [] AO308Convert(MessageObject mo)
        {
            const int maxNumberOfChannels = 1;

            double[] v = new double[maxNumberOfChannels];
            double ch0 = BitConverter.ToDouble( mo.OrigByteMessage, 16);
            v[0] = ch0;
            return v;
        }
    }
}
