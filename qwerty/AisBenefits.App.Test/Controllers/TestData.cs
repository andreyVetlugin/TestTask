using System.Collections.Generic;
using DataLayer.Entities;

namespace AisBenefits.App.Test.Controllers
{
    static class TestData
    {
        public static ICollection<IBenefitsEntity> Create()
        {
            return new List<IBenefitsEntity>();
        }
    }
}
