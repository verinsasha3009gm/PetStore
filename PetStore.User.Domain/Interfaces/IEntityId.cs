﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Interfaces
{
    public interface IEntityId<T>
    {
        public T Id { get; set; }
    }
}
