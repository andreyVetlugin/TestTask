using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GamesManager.Models
{
    public class GameEditForm
    {
        public Guid? Id { get; set; }
        [Display(Name = "Название игры"),Required(ErrorMessage = "Введите {0}")]
        public string Title { get; set; }
        [Display(Name = "Дата релиза"),Required(ErrorMessage = "Введите Дату релиза")]
        public DateTime ReleaseDate { get; set; } //тип ???? нужна ли проверка ///на существование ? 
        [Display(Name = "Жанры игры"), Required(ErrorMessage = "Введите хоть один игровой жанр")]
        public string GameGenres { get; set; } //???
        [Display(Name = "Наименование издателя"), Required(ErrorMessage = "Введите {0}")]
        public string Publisher { get; set; }
    }
}
