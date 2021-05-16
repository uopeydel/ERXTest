using ERXTest.Shared.DBModels;
using ERXTest.Shared.Helpers;
using ERXTest.Shared.Models;
using ERXTest.Shared.Models.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERXTest.BZ.Services
{
    public interface IDropDownService
    {
        Task<ERXTestResults<bool>> Upsert(DropDownUpsertRequest data);
        Task<ERXTestResults<DropDownModel>> DropDownItem(int dropDownId);
        Task<ERXTestResults<List<DropDownModel>>> List();
    }

    public class DropDownService : IDropDownService
    {
        private IHttpService _httpService;

        public DropDownService(IHttpService httpService)
        {
            _httpService = httpService;
        }



        public async Task<ERXTestResults<bool>> Upsert(DropDownUpsertRequest data)
        {
            ERXTestResults<bool> response =
                await _httpService.Post<ERXTestResults<bool>>(
                    "/api/DropDown/v1/Upsert", data);
            return response;
        }

        public async Task<ERXTestResults<DropDownModel>> DropDownItem(int dropDownId)
        {
            ERXTestResults<DropDownModel> response =
                await _httpService.Post<ERXTestResults<DropDownModel>>(
                    $"/api/DropDown/v1/DropDownItem/{dropDownId}", default);
            return response;
        }


        public async Task<ERXTestResults<List<DropDownModel>>> List()
        {

            ERXTestResults<List<DropDownModel>> response =
                await _httpService.Post<ERXTestResults<List<DropDownModel>>>(
                    $"/api/DropDown/v1/List", default);
            return response;
        }

    }
}