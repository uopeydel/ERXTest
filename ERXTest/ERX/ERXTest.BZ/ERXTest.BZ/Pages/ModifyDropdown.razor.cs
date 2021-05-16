using Blazored.Modal;
using Blazored.Modal.Services;
using ERXTest.BZ.Services;
using ERXTest.Shared.Models;
using ERXTest.Shared.Models.Request;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERXTest.BZ.Pages
{
    public partial class ModifyDropdown
    {
        [CascadingParameter]
        BlazoredModalInstance ModalInstance { get; set; }

        [CascadingParameter]
        public IModalService Modal { get; set; }

        public List<DropDownModel> DropDownList { get; set; }
        public DropDownModel DropDownItemData { get; set; }
        public string NameTemp { get; set; }

        protected override async Task OnInitializedAsync()
        {
            DropDownList = new List<DropDownModel>();
            DropDownItemData = new DropDownModel();
            DropDownItemData.DropDownItems = new List<DropDownItemModel>();
            await GetList();
        }

        public async Task GetList()
        {
            var listResult = await DropDownService.List();
            if (listResult.Error)
            {
                var formModal = Modal.Show<AlertModal>(listResult.Message.FirstOrDefault());
                return;
            }

            DropDownList = listResult.Data;
        }

        public async Task Clear()
        {
            DropDownItemData = new DropDownModel();
            DropDownItemData.DropDownItems = new List<DropDownItemModel>();
        }
        public async Task Upsert()
        {
            DropDownUpsertRequest data = new DropDownUpsertRequest
            {
                dropDown = DropDownItemData,
                dropDownItem = DropDownItemData.DropDownItems,
            };
            var listResult = await DropDownService.Upsert(data);
            if (listResult.Error)
            {
                var formModal = Modal.Show<AlertModal>(listResult.Message.FirstOrDefault());
                return;
            }

            Modal.Show<AlertModal>("Save success.");
            await GetList();

            var IdToRefresh = 0;
            if (data.dropDown.Id > 0)
            {
                IdToRefresh = data.dropDown.Id;
            }
            else
            {
                IdToRefresh = DropDownList.OrderByDescending(o => o.Id).Select(s => s.Id).FirstOrDefault();
            }

            await DropDownItem(IdToRefresh);
        }
        public async Task DropDownItem(int dropDownId)
        {
            var itemResult = await DropDownService.DropDownItem(dropDownId);
            if (itemResult.Error)
            {
                DropDownItemData = new DropDownModel();
                var formModal = Modal.Show<AlertModal>(itemResult.Message.FirstOrDefault());
                return;
            }

            DropDownItemData = itemResult.Data;

        }

        public async Task AddToList()
        {
            DropDownItemData.DropDownItems.Add(new DropDownItemModel { Name = NameTemp });
        }


        public async Task RemoveFromList(DropDownItemModel item)
        {
            DropDownItemData.DropDownItems.Remove(item);
        }


    }
}
