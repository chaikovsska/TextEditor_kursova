using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Strategy.EncodingStrategy
{
    public class HeuristicEncodingStrategy : IEncodingStrategy
    {
        public Encoding Detect(byte[] bytes)
        {
            try
            {
                var utf8 = Encoding.UTF8.GetString(bytes);
                if (utf8.Contains("�"))
                    return Encoding.GetEncoding(1251);
                return Encoding.UTF8;
            }
            catch
            {
                return Encoding.GetEncoding(1251);
            }
        }
    }
}
