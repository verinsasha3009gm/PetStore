using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Products.Domain.Dto.Category;
using PetStore.Products.Domain.Interfaces.Services;
using PetStore.Products.Domain.Result;

namespace PetStore.Products.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoryController:ControllerBase
    { 
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<CategoryDto>>> GetCategoryAsync(string categoryName)
        {
            var result = await _categoryService.GetCategoryAsync(categoryName);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<CategoryDto>>> GetAllCategoriesAsync()
        {
            var result = await _categoryService.GetAllCategories();
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<CategoryDto>>> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var result = await _categoryService.CreateCategoryAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<CategoryDto>>> UpdateCategoryAsync(UpdateCategoryDto dto)
        {
            var result = await _categoryService.UpdateCategoryAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{categoryName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<CategoryDto>>> DeleteCategoryAsync( string categoryName)
        {
            var result = await _categoryService.DeleteCategoryAsync(categoryName);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("AddProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<CategoryDto>>> AddProductInCategory(ProductInCategoryDto dto)
        {
            var result = await _categoryService.AddProductInCategoryAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameCategory"></param>
        /// <param name="nameProduct"></param>
        /// <returns></returns>
        [HttpDelete("RemoveProduct/{nameCategory}/{nameProduct}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<CategoryDto>>> RemoveProductInCategoryAsync(string nameCategory, string nameProduct)
        {
            var result = await _categoryService.RemoveProductInCategoryAsync(nameCategory,nameProduct);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
