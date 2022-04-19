using System;
using System.Collections.Generic;
using System.Text;

namespace MAV.Common.Models
{
    public class ResponseO<T> where T : class
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Result { get; set; }
    }
}
