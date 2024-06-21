using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Library.BaseInterfaces
{
    public interface IEntityId<T>
    {
        public T Id { get; }
    }
}
