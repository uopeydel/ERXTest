using AutoMapper;
using ERXTest.Server.Helper;
using ERXTest.Server.Repositories;
using ERXTest.Shared.DBModels;
using ERXTest.Shared.Helpers;
using ERXTest.Shared.Models;
using ERXTest.Shared.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERXTest.Server.Services
{
    public interface IDropDownService
    {
        Task<ERXTestResults<bool>> Upsert(DropDownUpsertRequest data);
        Task<ERXTestResults<DropDownModel>> DropDownItem(int dropDownId);
        Task<ERXTestResults<List<DropDownModel>>> List();
    }
    public class DropDownService : IDropDownService
    {
        private readonly IMapper _mapper;
        public readonly DateTime now;
        private readonly IDropDownRepository _DropDownRepository;
        private readonly ILoggerService _logger;
        public DropDownService(
            IDropDownRepository DropDownRepository,
            ILoggerService logger,
            IMapper mapper
        )
        {
            _mapper = mapper;
            _DropDownRepository = DropDownRepository;
            _logger = logger;
            now = DateTime.UtcNow.AddHours(7);
        }


        public async Task<ERXTestResults<bool>> Upsert(DropDownUpsertRequest data)
        {
            try
            {
                ERXTestResults<bool> result = await _DropDownRepository.Upsert(
                      _mapper.Map<DropDown>(data.dropDown),
                      _mapper.Map<List<DropDownItem>>(data.dropDownItem)
                     );
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<bool>("Error At Upsert service", ex);
            }
        }

        public async Task<ERXTestResults<DropDownModel>> DropDownItem(int dropDownId)
        {
            try
            {
                ERXTestResults<DropDownModel> result = await _DropDownRepository.DropDownItem(dropDownId);
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<DropDownModel>("Error At DropDownItem service", ex);
            }
        }

        public async Task<ERXTestResults<List<DropDownModel>>> List()
        {
            try
            {
                ERXTestResults<List<DropDownModel>> result = await _DropDownRepository.List();
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<List<DropDownModel>>("Error At List service", ex);
            }
        }
    }

}
