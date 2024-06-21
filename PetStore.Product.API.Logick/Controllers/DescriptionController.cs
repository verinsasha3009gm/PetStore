using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Products.Application.Services;
using PetStore.Products.Domain.Dto.Description;
using PetStore.Products.Domain.Interfaces.Services;
using PetStore.Products.Domain.Result;

namespace PetStore.Products.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]/")]
    public class DescriptionController : Controller
    {
        private readonly IDescriptionService _descriptionService;
        public DescriptionController(IDescriptionService descriptionService)
        {
            _descriptionService = descriptionService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<DescriptionDto>>> GetDescriptionAsync(string Name, string Culture)
        {
            var result = await _descriptionService.GetDescriptionCultureAsync(Name,Culture);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prodName"></param>
        /// <returns></returns>
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<DescriptionDto>>> GetDescriptionsAsync(string prodName)
        {
            var result = await _descriptionService.GetDescriptionsAsync(prodName);
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
        public async Task<ActionResult<BaseResult<DescriptionDto>>> CreateDescriptionAsync(DescriptionCultureDto dto)
        {
            var result = await _descriptionService.AddDescriptionCultureAsync(dto);
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
        public async Task<ActionResult<BaseResult<DescriptionDto>>> UpdateDescriptionAsync(DescriptionCultureDto dto)
        {
            var result = await _descriptionService.UpdateDescriptionCultureAsync(dto);
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
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<DescriptionDto>>> DeleteDescriptionsAsync(string prodName, string culture)
        {
            var result = await _descriptionService.RemoveDescriptionCultureAsync(prodName,culture);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
