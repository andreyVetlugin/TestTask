﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesManager.Infrastructure.Services;
using GamesManager.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GamesManager.Services.Handlers.Games
{
    public class EditFormRemoveHandler: IEditFormRemoveHandler
    {
        public OperationResult Handle(GameEditForm form, ModelStateDictionary modelState)
        {
            throw new NotImplementedException();
        }
    }
}