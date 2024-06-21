using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Dto.Market
{
    public record MarketProductLineDto(string NameProduct, long Count, string MarketGuidId);
}
