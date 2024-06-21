using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PetStore.Products.Application.Resources;
using PetStore.Products.Domain.Dto.Teg;
using PetStore.Products.Domain.Entity;
using PetStore.Products.Domain.Enum;
using PetStore.Products.Domain.Interfaces.Repository;
using PetStore.Products.Domain.Interfaces.Services;
using PetStore.Products.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Products.Application.Services
{
    public class TegService : ITegService
    {
        private readonly IBaseRepository<Teg> _tegRepository;
        private readonly IMapper _mapper;
        public TegService(IBaseRepository<Teg> tegRepository, IMapper mapper)
        {
            _tegRepository = tegRepository;
            _mapper = mapper;
        }
        public async Task<BaseResult<TegDto>> CreateTegAsync(TegDto dto)
        {
            var teg = await _tegRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == dto.Name);
            if (teg != null)
            {
                return new BaseResult<TegDto>
                {
                    ErrorMessage = ErrorMessages.TegAlreadyExists,
                    ErrorCode =(int)ErrorCodes.TegAlreadyExists,
                };
            }
            try
            {
                await _tegRepository.CreateAsync(new() { Name = dto.Name});
            }
            catch (Exception ex)
            {
                return new BaseResult<TegDto>
                {
                    ErrorCode = (int)ErrorCodes.ErrorTegCreate,
                    ErrorMessage = ErrorMessages.ErrorTegCreate,
                };
            }
            return new BaseResult<TegDto>
            {
                Data = dto
            };
        }

        public async Task<BaseResult<TegDto>> DeleteTegAsync(string Name)
        {
            var teg = await _tegRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == Name);
            if (teg == null)
            {
                return new BaseResult<TegDto>
                {
                    ErrorCode = (int)ErrorCodes.TegIsNotFound,
                    ErrorMessage = ErrorMessages.TegIsNotFound,
                };
            }
            try
            {
                _tegRepository.DeleteAsync(teg);
                await _tegRepository.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return new BaseResult<TegDto>
                {
                    ErrorCode = (int)ErrorCodes.ErrorTegDelete,
                    ErrorMessage = ErrorMessages.ErrorTegDelete,
                };
            }
            return new BaseResult<TegDto>
            {
                Data = _mapper.Map<TegDto>(teg)
            };
        }

        public async Task<CollectionResult<TegDto>> GetAllTags()
        {
            var tegs = await _tegRepository.GetAll().Select(p=>new TegDto(p.Name)).ToArrayAsync();
            if (tegs.Any(p=> p == null))
            {
                return new CollectionResult<TegDto>()
                {
                    ErrorMessage = ErrorMessages.ErrorAllTegs,
                    ErrorCode =(int) ErrorCodes.ErrorAllTegs
                };
            }
            return new CollectionResult<TegDto>
            {
                Data = tegs,
                Count = tegs.Length
            };
        }

        public async Task<BaseResult<TegDto>> GetTegsByIdAsync(string Name)
        {
            var teg = await _tegRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == Name);
            if(teg == null)
            {
                return new BaseResult<TegDto>
                {
                    ErrorCode = (int)ErrorCodes.TegIsNotFound,
                    ErrorMessage = ErrorMessages.TegIsNotFound,
                };
            }
            return new BaseResult<TegDto>
            {
                Data = _mapper.Map<TegDto>(teg),
            };
        }

        public async Task<BaseResult<TegDto>> UpdateTegAsync(UpdateTegDto dto)
        {
            var teg = await _tegRepository.GetAll().FirstOrDefaultAsync(p=>p.Name == dto.LastName);
            if(teg == null)
            {
                return new BaseResult<TegDto>
                {
                    ErrorCode = (int)ErrorCodes.TegIsNotFound,
                    ErrorMessage = ErrorMessages.TegIsNotFound
                };
            }
            teg.Name = dto.NewName;
            _tegRepository.UpdateAsync(teg);
            await _tegRepository.SaveChangesAsync();
            return new BaseResult<TegDto>
            {
                Data = _mapper.Map<TegDto>(teg)
            };
        }
    }
}
