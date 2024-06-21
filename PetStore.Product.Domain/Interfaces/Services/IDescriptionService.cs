using PetStore.Products.Domain.Dto.Description;
using PetStore.Products.Domain.Result;

namespace PetStore.Products.Domain.Interfaces.Services
{
    public interface IDescriptionService
    {
        /// <summary>
        /// Извлечение нужного описания у продукта 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<DescriptionDto>> GetDescriptionCultureAsync(string Name, string Culture);
        /// <summary>
        /// Извлечение всего описания у продукта
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        Task<CollectionResult<DescriptionDto>> GetDescriptionsAsync(string prod);
        /// <summary>
        /// Обновление описания у продукта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<DescriptionDto>> UpdateDescriptionCultureAsync(DescriptionCultureDto dto);
        /// <summary>
        /// Создание нового описания у продукта
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<DescriptionDto>> AddDescriptionCultureAsync(DescriptionCultureDto dto);
        /// <summary>
        /// Удаление описания у продукта 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<BaseResult<DescriptionDto>> RemoveDescriptionCultureAsync(string prodName, string culture);
    }
}
