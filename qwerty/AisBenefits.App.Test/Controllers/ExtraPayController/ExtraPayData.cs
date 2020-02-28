using DataLayer.Entities;
using System;
using System.Collections.Generic;

namespace AisBenefits.App.Test.Controllers.ExtraPayController
{
    static class ExtraPayData
    {
        public static Guid PersonInfoId = Guid.NewGuid();

        public static ICollection<IBenefitsEntity> AddFullExtraPayData(this ICollection<IBenefitsEntity> collection, Guid organizationId)
        {
            collection.Add(new PersonInfo
            {
                Id = PersonInfoId,
                RootId = PersonInfoId,
                CreateTime = new DateTime(2000, 1, 1),

                Sex = 'ж'
            });

            collection.Add(new ExtraPay
            {
                Id = Guid.NewGuid(),
                PersonRootId = PersonInfoId,

                Salary = 1000,
                MaterialSupport = 1000,
                UralMultiplier = 1.15m,
                Premium = 1000,
            });

            var workInfoId = Guid.NewGuid();
            collection.Add(new WorkInfo
            {
                Id = workInfoId,
                RootId = workInfoId,

                OrganizationId = organizationId,

                PersonInfoRootId = PersonInfoId,
                StartDate = new DateTime(2000, 1, 1),
                EndDate = new DateTime(2005, 1, 1)
            });

            return collection;
        }

        public static ICollection<IBenefitsEntity> AddInitialExtraPayData(this ICollection<IBenefitsEntity> collection, Guid organizationId)
        {
            collection.Add(new PersonInfo
            {
                Id = PersonInfoId,
                RootId = PersonInfoId,
                CreateTime = new DateTime(2000, 1, 1),

                Sex = 'ж'
            });

            var workInfoId = Guid.NewGuid();
            collection.Add(new WorkInfo
            {
                Id = workInfoId,
                RootId = workInfoId,

                OrganizationId = organizationId,

                PersonInfoRootId = PersonInfoId,
                StartDate = new DateTime(2000, 1, 1),
                EndDate = new DateTime(2005, 1, 1)
            });

            return collection;
        }

        public static ICollection<IBenefitsEntity> AddDefinedExtraPayData(this ICollection<IBenefitsEntity> collection, Guid organizationId)
        {
            collection.Add(new PersonInfo
            {
                Id = PersonInfoId,
                RootId = PersonInfoId,
                CreateTime = new DateTime(2000, 1, 1),

                Sex = 'ж'
            });

            collection.Add(new ExtraPay
            {
                Id = Guid.NewGuid(),
                PersonRootId = PersonInfoId,

                Salary = 1000,
                MaterialSupport = 1000,
                UralMultiplier = 1.15m,
                Premium = 1000,

                TotalExtraPay = 3400
            });

            var workInfoId = Guid.NewGuid();
            collection.Add(new WorkInfo
            {
                Id = workInfoId,
                RootId = workInfoId,

                OrganizationId = organizationId,

                PersonInfoRootId = PersonInfoId,
                StartDate = new DateTime(2000, 1, 1),
                EndDate = new DateTime(2005, 1, 1)
            });

            collection.Add(new Solution
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = PersonInfoId,
                Type = SolutionType.Pereraschet,
                Comment = "Определить"
            });

            return collection;
        }

        public static ICollection<IBenefitsEntity> AddPausedExtraPayData(this ICollection<IBenefitsEntity> collection, Guid organizationId)
        {
            collection.Add(new PersonInfo
            {
                Id = PersonInfoId,
                RootId = PersonInfoId,
                CreateTime = new DateTime(2000, 1, 1),

                Sex = 'ж',

                StoppedSolutions = true
            });

            collection.Add(new ExtraPay
            {
                Id = Guid.NewGuid(),
                PersonRootId = PersonInfoId,

                Salary = 1000,
                MaterialSupport = 1000,
                UralMultiplier = 1.15m,
                Premium = 1000,

                TotalExtraPay = 3400
            });

            var workInfoId = Guid.NewGuid();
            collection.Add(new WorkInfo
            {
                Id = workInfoId,
                RootId = workInfoId,

                OrganizationId = organizationId,

                PersonInfoRootId = PersonInfoId,
                StartDate = new DateTime(2000, 1, 1),
                EndDate = new DateTime(2005, 1, 1)
            });

            collection.Add(new Solution
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = PersonInfoId,
                Type = SolutionType.Opredelit,
                OutdateTime = new DateTime(2001, 1, 1),
                Comment = "Определить"
            });
            collection.Add(new Solution
            {
                Id = Guid.NewGuid(),
                PersonInfoRootId = PersonInfoId,
                Type = SolutionType.Pause,
                Comment = "Приостановить"
            });

            return collection;
        }
    }
}
