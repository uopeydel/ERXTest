using AutoMapper;
using ERXTest.Shared.DBModels;
using ERXTest.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERXTest.Server.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<DropDownItem, DropDownItemModel>().ReverseMap();
            CreateMap<DropDown, DropDownModel>().ReverseMap();
            CreateMap<Answer, AnswerModel>().ReverseMap();
            CreateMap<Question, QuestionModel>().ReverseMap();
            CreateMap<Respondent, RespondentModel>().ReverseMap();

            //CreateMap<List<DropDownItem>, List<DropDownItemModel>>().ReverseMap();
            //CreateMap<List<DropDownItemModel>, List<DropDownItem>>().ReverseMap();
            //CreateMap<List<DropDown>, List<DropDownModel>>().ReverseMap();
            //CreateMap<List<Answer>, List<AnswerModel>>().ReverseMap();
            //CreateMap<List<Question>, List<QuestionModel>>().ReverseMap();
            //CreateMap<List<Respondent>, List<RespondentModel>>().ReverseMap();
        }
    }
}
