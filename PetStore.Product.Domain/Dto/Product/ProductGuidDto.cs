﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Dto.Product
{
    public record ProductGuidDto(string GuidId,string Name,string Description,decimal Price);
}
