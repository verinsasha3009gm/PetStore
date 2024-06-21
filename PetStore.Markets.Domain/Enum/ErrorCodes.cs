using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Markets.Domain.Enum
{
    public enum ErrorCodes
    {
        AddressAlreadyExists = 1,
        AddressNotFound = 2,
        AddressCreateError = 3,
        AddressDeleteError = 4,
        AddressUpdateError = 5,

        EmployeAlreadyExists = 11,
        EmployeNotFound = 12,
        EmployeCreateError = 13,

        EmployeDeleteError = 14,
        EmployeUpdateError = 15,

        UserNotFound = 21,
        UserAlreadyExists = 22,
        UserCreateError = 23,
        UserUpdateError = 24,
        UserDeleteError = 25,

        MarketNotFound = 31,
        MarketAlreadyExists = 32,
        MarketCreateError = 33,
        MarketUpdateError = 34,
        MarketDeleteError = 35,

        EmployePassportNotFound = 41,
        EmployePassportAlreadyExists = 42,
        EmployePassportCreateError = 43,

        EmployePassportDeleteError = 45,
        EmployePassportUpdateError = 46,

        PasswordIsNotValid = 51,

        PassportNotFound = 61,
        PassportAlreadyExists = 62,
        PassportCreateError = 63,
        PassportUpdateError = 64,
        PassportDeleteError = 65,

        ProductNotFound = 71,
        ProductAlreadyExists = 72,
        ProductCreateError = 73,
        ProductUpdateError = 74,
        ProductDeleteError = 75,

        MarketCapitalNotFound = 81,
        MarketCapitalAlreadyExists = 82,

        MarketCapitalCreateError = 83,
        MarketCapitalDeleteError = 84,
        MarketCapitalUpdateError = 85,

        ProductLineNotFound = 91,
        ProductLineAlreadyExists = 92,
        ProductLineUpdateError = 93,
        ProductLineCreateError = 94,
        ProductLineDeleteError = 95,

        InternalServerException = 99
    }
}
