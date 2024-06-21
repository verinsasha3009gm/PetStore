using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Dto.MarketCapital
{
    public record MarketCapitalProductLineDto(string NameProduct,string Day,int Count , string guidId);
}
