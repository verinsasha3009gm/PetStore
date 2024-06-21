﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Result
{
    public class CollectionResult<T> : BaseResult<IEnumerable<T>>
    {
        public long Count;
    }
}
