using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Domain.Enum
{
    public enum ErrorCodes
    {
        ErrorGetProduct = 1,
        ErrorCreateProduct = 2,
        ErrorUpdateProduct = 3,
        ErrorDeleteProduct = 4,
        ErrorGetAllProduct = 5,
        ErrorGetAllProductsInCategory = 6,
        ProductAlreadyExists = 7,
        ProductIsNotFound = 8,

        CategoryIsNotFound = 11,
        CategoryAlreadyExists = 12,
        ErrorGetCategory = 13,
        ErrorCreateCategory = 14,
        ErrorUpdateCategory = 15,
        ErrorDeleteCategory = 16,
        ErrorGetAllCategories = 17,

        ErrorGetProductInCategory = 21,
        ErrorCreateProductInCategory = 22,
        ErrorUpdateProductInCategory = 23,
        ErrorDeleteProductInCategory = 24,

        ErrorCreateDescription = 30,
        ErrorUpdateDescription = 31,
        ErrorDeleteDescription = 32,
        ErrorGetDescription = 33,
        DescriptionNotFound = 34,
        DescriptionAlreadyExists = 35,

        ErrorTegGet = 41,
        ErrorTegUpdate = 42,
        ErrorTegDelete = 43,
        ErrorTegCreate = 44,
        TegIsNotFound = 45,
        TegAlreadyExists = 46,
        ErrorAllTegs = 47,

        ErrorGetProductPassport = 51,
        ErrorCreateProductPassport = 52,
        ErrorUpdateProductPassport = 53,
        ErrorDeleteProductPassport =54,
        ProductPassportNotFound = 55,
        ProductPassportAlreadyExists =56,

        InternalServerException = 57,
    }
}
