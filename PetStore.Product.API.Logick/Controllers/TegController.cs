using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Products.Domain.Dto.Teg;
using PetStore.Products.Domain.Interfaces.Services;
using PetStore.Products.Domain.Result;

namespace PetStore.Products.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]/")]
    public class TegController : Controller
    {
        private readonly ITegService _tegService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tegService"></param>
        public TegController(ITegService tegService)
        {
            _tegService = tegService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        public async Task<ActionResult<CollectionResult<TegDto>>> GetAllTegsAsync()
        {
            var result = await _tegService.GetAllTags();
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
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<TegDto>>> GetTegAsync(string Name)
        {
            var result = await _tegService.GetTegsByIdAsync(Name);
            if(result.IsSucces)
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
        public async Task<ActionResult<BaseResult<TegDto>>> CreateTegAsync(TegDto dto)
        {
            var result = await _tegService.CreateTegAsync(dto);
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
        public async Task<ActionResult<BaseResult<TegDto>>> UpdateTegAsync(UpdateTegDto dto)
        {
            var result = await _tegService.UpdateTegAsync(dto);
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
        public async Task<ActionResult<BaseResult<TegDto>>> DeleteTegAsync(string Name)
        {
            var result = await _tegService.DeleteTegAsync(Name);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        
    }
}
