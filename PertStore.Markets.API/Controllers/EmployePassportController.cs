using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Markets.DAL.Repository;
using PetStore.Markets.Domain.Dto.EmployePassport;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;

namespace PetStore.Markets.API.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmployePassportController : ControllerBase
    {
        private readonly IEmployePassportService _employePassportService;
        public EmployePassportController(IEmployePassportService employePassportService)
        {
            _employePassportService = employePassportService;
        }
        [HttpGet("EmpPassport")]
        public async Task<ActionResult<BaseResult<EmployePassportGuidDto>>> GetEmployePassportAsync(string Email)
        {
            var result = await _employePassportService.GetEmployePassportAsync(Email);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("EmpPassportGuid")]
        public async Task<ActionResult<BaseResult<EmployePassportDto>>> GetEmployePassportGuidAsync(string EmployePassportGuid)
        {
            var result = await _employePassportService.GetEmployePassportGuidAsync(EmployePassportGuid);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("Create")]
        public async Task<ActionResult<BaseResult<EmployePassportDto>>> CreateEmployePassportAsync(CreateEmployePassportDto dto)
        {
            var result = await _employePassportService.CreateEmployePassportAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPost("CreateInEmploye")]
        public async Task<ActionResult<BaseResult<EmployePassportDto>>> CreateEmployePassportInEmployeAsync(EmployePassportGuidDto dto)
        {
            var result = await _employePassportService.CreateEmployePassportInEmployeAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpPut]
        public async Task<ActionResult<BaseResult<EmployePassportDto>>> UpdateEmployePassportAsync(UpdateEmployePassportDto dto)
        {
            var result = await _employePassportService.UpdateEmployePassportAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpDelete]
        public async Task<ActionResult<BaseResult<EmployePassportDto>>> DeleteEmployePassportAsync(string Email,string Password)
        {
            var result = await _employePassportService.DeleteEmployePassportAsync(Email,Password);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
