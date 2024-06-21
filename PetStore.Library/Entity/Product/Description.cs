using PetStore.Library.BaseInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Library.Entity.Product
{
    public class Description : IEntityId<long>, IAuditable
    {
        public long Id { get; set; }
        public string Culture { get; set; }
        public string Detail { get; set; }
        public Product Product { get; set; }
        public long ProductId { get; set; }

        public DateTime? CreatedAt { get; set; }
        public long CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long UpdatedBy { get; set; }
    }
}
