using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Strategy.EncodingStrategy
{
    public class AsciiEncodingStrategy : IEncodingStrategy
    {
        public Encoding Detect(byte[] bytes)
        {
            bool isAscii = true;

            foreach (byte b in bytes)
            {
                if (b > 127) 
                {
                    isAscii = false;
                    break;
                }
            }

            return isAscii ? Encoding.ASCII : null;
        }
    }
}
