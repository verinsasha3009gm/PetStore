﻿using PetStore.Users.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Entity
{
    public class UserRole : IEntityId<long>
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        
    }
}
