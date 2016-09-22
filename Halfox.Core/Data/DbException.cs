using System;
using System.Runtime.Serialization;

namespace Halfox.Core
{
    /// <summary>
    /// Halfox���ݿ��쳣
    /// </summary>
    [Serializable]
    public class DbException : HaException
    {
        public DbException() : base() { }

        public DbException(string message) : base(message) { }

        public DbException(string message, Exception inner) : base(message, inner) { }

        public DbException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
