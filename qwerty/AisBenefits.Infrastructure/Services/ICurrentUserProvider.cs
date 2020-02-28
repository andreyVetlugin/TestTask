using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AisBenefits.Infrastructure.Services
{
    public interface ICurrentUserProvider
    {
        User GetCurrentUser();
    }
}
