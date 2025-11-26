using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Strategy.EncodingStrategy
{
    public class EncodingDetector
    {
        private readonly List<IEncodingStrategy> _strategies;

        public EncodingDetector()
        {
            _strategies = new List<IEncodingStrategy>
        {
            new AsciiEncodingStrategy(),   
            new BomEncodingStrategy(),
            new HeuristicEncodingStrategy()
        };
        }

        public Encoding DetectEncoding(byte[] bytes)
        {
            foreach (var strategy in _strategies)
            {
                var encoding = strategy.Detect(bytes);
                if (encoding != null)
                    return encoding;
            }
            return Encoding.UTF8; 
        }
    }
}
