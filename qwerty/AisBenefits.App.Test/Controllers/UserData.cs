using System;
using System.Collections.Generic;
using DataLayer.Entities;

namespace AisBenefits.App.Test.Controllers
{
    static class UserData
    {
        public static Guid UserId = Guid.NewGuid();

        public static ICollection<IBenefitsEntity> AddUserData(this ICollection<IBenefitsEntity> collection)
        {
            collection.Add(new User
            {
                Id = UserId,
                Login = "user",
                Password = "user"
            });

            return collection;
        }
    }
}
