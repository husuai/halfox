using System;
using System.Runtime.Serialization;

namespace Halfox.Core
{
    /// <summary>
    /// Halfox“Ï≥£¿‡
    /// </summary>
    [Serializable]
    public class HaException : ApplicationException
    {
        public HaException() { }

        public HaException(string message) : base(message) { }

        public HaException(string message, Exception inner) : base(message, inner) { }

        protected HaException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
