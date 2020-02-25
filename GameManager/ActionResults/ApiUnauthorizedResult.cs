using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesManager.ActionResults
{
    public class ApiUnauthorizedResult: ApiResult
    {

        public ApiUnauthorizedResult()
            : base(new { Message = "Пожалуйста, сначала авторизуйтесь" }, 401)
        {
        }

    }
}
