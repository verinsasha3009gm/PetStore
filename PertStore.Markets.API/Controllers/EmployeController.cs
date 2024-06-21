using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using PetStore.Markets.DAL.Repository;
using PetStore.Markets.Domain.Dto;
using PetStore.Markets.Domain.Dto.Employe;
using PetStore.Markets.Domain.Entity;
using PetStore.Markets.Domain.Interfaces.Service;
using PetStore.Markets.Domain.Result;

namespace PetStore.Markets.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmployeController : ControllerBase
    {
        private readonly IEmployeService _employeService;
        public EmployeController(IEmployeService employeService)
        {
            _employeService = employeService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        public async Task<ActionResult<CollectionResult<EmployeDto>>> GetAllEmployeAsync()
        {
            var result = await _employeService.GetAllEmployesAsync();
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpGet("Get")]
        public async Task<ActionResult<BaseResult<EmployeDto>>> GetEmployeAsync(string Email)
        {
            var result = await _employeService.GetEmployeAsync(Email);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpGet("GetGuid")]
        public async Task<ActionResult<BaseResult<EmployeGuidDto>>> GetEmployeGuidIdAsync(string Email,string Password)
        {
            var result = await _employeService.GetEmployeGuidIdAsync(Email, Password);
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
        [HttpPost("Login")]
        public async Task<ActionResult<BaseResult<TokenDto>>> LoginEmployeAsync(LoginEmployeDto dto)
        {
            var result = await _employeService.LoginEmployeAsync(dto);
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
        [HttpPost("Registration")]
        public async Task<ActionResult<BaseResult<EmployeDto>>> RegistrationEmployeAsync(RegistrationEmployeDto dto)
        {
            var result = await _employeService.RegistrationEmployeAsync(dto);
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
        [HttpPut]
        public async Task<ActionResult<BaseResult<EmployeDto>>> UpdateEmployeAsync(UpdateEmployeDto dto)
        {
            var result = await _employeService.UpdateEmployeAsync(dto);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        } 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<BaseResult<EmployeDto>>> DeleteEmployeAsync(string Email, string Password)
        {
            var result = await _employeService.DeleteEmployeAsync(Email, Password);
            if (result.IsSucces)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
