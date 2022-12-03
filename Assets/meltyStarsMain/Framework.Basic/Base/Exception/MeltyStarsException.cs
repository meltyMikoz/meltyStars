using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeltyStars
{
    public class MeltyStarsException : Exception
    {
        public MeltyStarsException() { }
        public MeltyStarsException(string message) : base(message) { }
    }
}
