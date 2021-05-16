using ERXTest.Server.Services;
using ERXTest.Shared.Helpers;
using ERXTest.Shared.Models;
using ERXTest.Shared.Models.Request;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
namespace ERXTest.Server.Facade
{
    public class DropDownFacade
    {
        private readonly IDropDownService _DropDownService;
        public DropDownFacade(
              IDropDownService DropDownService)
        {
            _DropDownService = DropDownService;
        }


        public async Task<ERXTestResults<bool>> Upsert(DropDownUpsertRequest data)
        {
            try
            {
                ERXTestResults<bool> result = await _DropDownService.Upsert(data);
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<bool>("Error At Upsert facade", ex);
            }
        }

        public async Task<ERXTestResults<DropDownModel>> DropDownItem(int dropDownId)
        {
            try
            {
                ERXTestResults<DropDownModel> result = await _DropDownService.DropDownItem(dropDownId);
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<DropDownModel>("Error At DropDownItem facade", ex);
            }
        }

        public async Task<ERXTestResults<List<DropDownModel>>> List()
        {
            try
            {
                ERXTestResults<List<DropDownModel>> result = await _DropDownService.List();
                return result;
            }
            catch (Exception ex)
            {
                return ERXTestResponse.CreateErrorResponse<List<DropDownModel>>("Error At DropDownItem facade", ex);
            }
        }
    }
}
