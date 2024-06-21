using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Markets.Domain.Dto.Passport;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;

namespace PetStore.Markets.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PassportController : ControllerBase
    {
        private readonly IPassportService _passportService;
        public PassportController(IPassportService passportService)
        {
            _passportService = passportService;
        }
        [HttpGet("{PassportNumber}/{PassportSeria}")]
        public async Task<ActionResult<BaseResult<PassportDto>>> GetPassportAsync(string PassportSeria,long PassportNumber)
        {
            var result = await _passportService.GetPassportAsync(PassportSeria, PassportNumber);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("Name/{Name}/{Familien}")]
        public async Task<ActionResult<CollectionResult<PassportDto>>> GetPassportNameAsync(string Name, string Familien)
        {
            var result = await _passportService.GetPassportNameAsync(Name,Familien);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost]
        public async Task<ActionResult<BaseResult<PassportDto>>> CreatePassportAsync(CreatePassportDto dto)
        {
            var result = await _passportService.CreatePassportAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut]
        public async Task<ActionResult<BaseResult<PassportDto>>> UpdatePassportAsync(UpdatePassportDto dto)
        {
            var result = await _passportService.UpdatePassportAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete]
        public async Task<ActionResult<BaseResult<PassportDto>>> DeletePassportAsync(string PassportSeria, long PassportNumber)
        {
            var result = await _passportService.DeletePassportAsync(PassportSeria,PassportNumber);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
