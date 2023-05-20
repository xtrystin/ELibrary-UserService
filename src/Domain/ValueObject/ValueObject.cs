using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELibrary_UserService.Domain.ValueObject
{
    public abstract record ValueObject<T>
    {
        protected T _value;

        public T Value => _value;
    }
}
