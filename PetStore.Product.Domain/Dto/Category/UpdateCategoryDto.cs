﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Dto.Category
{
    public record UpdateCategoryDto(string Name,string NewName,string Description);
}
