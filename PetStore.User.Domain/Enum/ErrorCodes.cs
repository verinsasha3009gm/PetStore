using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Enum
{
    public enum ErrorCodes
    {
        InvalidToken = 1,
        InvalidClientRequest = 2,
        InternalServerException = 3,

        TheSpecifiedRegionCannotBeEntered = 11,
        ItIsNotPossibleToSpecifyCity = 12,
        ItIsNotPossibleToSpecifyStreet = 13,
        TheSpecifiedCountryCannotBeEntered = 14,
        AddressAlreadyExists = 15,
        AddressNotFound = 16,

        UserNotFound = 21,
        UserAlreadyExists = 22,
        PassportNotValid =23,
        PasswordNotMatch = 24,

        ProductNotFound = 31,
        ProductAlreadyExists = 32,
        ProductNotFoundInCart = 33,

        CartNotExist = 41,

        RoleAlreadyExists = 51,
        RoleNotFound = 52,

        UserRoleNotFound = 61,
        UserRoleAlreadyExists = 62
    }
}
