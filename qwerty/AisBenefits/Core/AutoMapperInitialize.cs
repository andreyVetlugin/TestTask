using System;
using System.Collections.Generic;
using AisBenefits.Infrastructure.DTOs;
using AisBenefits.Infrastructure.Services.ExtraPays;
using AisBenefits.Models.ExtraPayVariants;
using AisBenefits.Models.PersonInfos;
using AisBenefits.Models.Reestrs;
using AisBenefits.Models.Solutions;
using AutoMapper;
using DataLayer.Entities;

namespace AisBenefits.Core
{
    public static class AutoMapper
    {
        public static void Initialize()
        {
            Mapper.Initialize(cfg => {
                cfg.CreateMap<IPersonInfoDTO, PersonInfo>();
                cfg.CreateMap<IWorkInfoDTO, WorkInfo>();
                cfg.CreateMap<WorkInfo, IWorkInfoDTO>();
                cfg.CreateMap<PersonInfo, PersonInfoModel>();
                cfg.CreateMap<PersonInfo, PersonInfoWithWorkDTO>();
                cfg.CreateMap<PersonInfo, PersonInfoPreviewModel>();
                cfg.CreateMap<PersonInfo, PersonInfo>();
                cfg.CreateMap<WorkInfo, WorkInfo>();
                cfg.CreateMap<WorkInfo, WorkPlaceModel>();
                cfg.CreateMap<PersonInfoWithWorkDTO, PersonInfoModel>();
                cfg.CreateMap<IExtraPayEditForm, ExtraPay>();
                cfg.CreateMap<ExtraPay, ExtraPay>();
                cfg.CreateMap<ExtraPayVariantEditForm, ExtraPayVariant>();
                cfg.CreateMap<ISolutionForm, Solution>();
                cfg.CreateMap<Solution, SolutionModel>();
                cfg.CreateMap<IPersonBankCardDto, PersonBankCard>();
                cfg.CreateMap<PersonBankCard, PersonBankCardModel>();
                cfg.CreateMap<IReestrDTO, Reestr>();
                cfg.CreateMap<Reestr, ReestrModel>();
                cfg.CreateMap<ReestrElement, ReestrElemModel>();
                cfg.CreateMap<Solution, Solution>();
                cfg.CreateMap<RecountDebt, RecountDebt>();
            });
        }
    }
}
