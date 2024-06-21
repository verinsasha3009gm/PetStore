using PetStore.Users.Domain.Result;
using PetStore.Users.Domain.Dto.Cart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Users.Domain.Interfaces.Services
{
    public interface ICartService
    {
        /// <summary>
        /// метод считывающий корзину пользователя
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        Task<BaseResult<CartDto>> GetUserCartAsync(string userLogin);
        /// <summary>
        /// Очистка корзины пользователя
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        Task<BaseResult<CartDto>> ClearUserCartAsync(string userLogin);
        /// <summary>
        /// метод считывающий все продукты у пользователя
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        Task<CollectionResult<CartLineDto>> GetUserAllCartLinesAsync(string userLogin);
        /// <summary>
        /// метод добавления продукта в корзину пользователя
        /// </summary>
        /// <param name="cartLine"></param>
        /// <returns></returns>
        Task<BaseResult<CartLineDto>> AddUserCartLineAsync(CartLineUserDto cartLine);
        /// <summary>
        /// метод удаления продукта из корзины пользователя
        /// </summary>
        /// <param name="guidId"></param>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        Task<BaseResult<CartLineDto>> RemoveUserCartLineAsync(string guidId, string userLogin);
    }
}
