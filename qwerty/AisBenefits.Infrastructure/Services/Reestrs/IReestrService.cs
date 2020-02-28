using AisBenefits.Infrastructure.DTOs;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AisBenefits.Infrastructure.Services.Reestrs
{
    public interface IReestrService
    {
        // Reestr InitOrGet(IReestrDTO reestrDTO);
        Reestr Init(IReestrDTO reestrDTO);
        Reestr Get();

        Reestr GetReestrById(Guid reestrId);
        Reestr Complete(Guid reestrId);
        List<Reestr> GetArchive(int year, int month);
        List<Reestr> GetArchive();
        //OperationResult SaveReestr(Guid reestrId);

        List<ReestrElement> GetReestrElements(Guid reestrId);
        Guid DeleteFromReestr(Guid reestrElemId); // ВОзвращает АЙДИ реестра, к которому принадлежит этот элемент
        Guid ReCountElementFromReestr(IRecountReestrElementDTO reestrElemForm, bool saveDb = true);
        Guid ReCountAllElementsFromReestr(IRecountReestrElementDTO[] reestrElemForms);
    }
}
