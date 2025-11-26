using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Strategy.EncodingStrategy
{
    public interface IEncodingStrategy
    {
        Encoding Detect(byte[] bytes);
    }
}
