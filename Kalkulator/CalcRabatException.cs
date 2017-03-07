using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalkulator
{
    [Serializable]
    public class CalcRabatException : Exception
    {
        public CalcRabatException() { }
        public CalcRabatException(string message) : base(message) { }
        public CalcRabatException(string message, Exception inner) : base(message, inner) { }
        protected CalcRabatException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
