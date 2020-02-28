using System;

namespace DataLayer.Entities
{
    public class PostInfoLog: IBenefitsEntity
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public DateTime Date { get; set; }


        public PostOperationType Operation { get; set; }

        public Guid EntityRootId { get; set; }

    }


    public enum PostOperationType : byte
    {
        PersonInfoCREATE = 10,
        PersonInfoUPDATE = 11,
        PersonInfoPauseSolution,
        PersonInfoResumeSolution,

        WorkInfoCREATE = 20,
        WorkInfoUPDATE = 21,

        ExtraPayEdit = 30,
        ExtraPayRecalculate,
        ExtraPayMassRecalculate,

        SolutionOpredelit = 40,
        SolutionPereraschet,
        SolutionPause,
        SolutionResume,
        SolutionStop,
        DeleteSolution,


        PersonBankCardCreate = 50,
        PersonBankCardUpdate = 51,

        IncomePensionApproved = 60,
        IncomePensionDeclined = 61,
        IncomePensionExtraPUpdate,

        ReestrCreate = 70,
        ReestrComplete,
        ReCountReestrElement,
        DeleteFromReestr,
        CreateReestrElement,

        MinExtraEdit = 80,

        OrganizationCreate = 90,
        OrganizationEdit,

        PfrSnilsUpdate = 100
    }

}
