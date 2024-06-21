using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Interfaces.DataBase
{
    public interface IStateSaveChanges
    {
        Task<int> SaveChangesAsync();
    }
}
