using System;
using System.ComponentModel.DataAnnotations;

namespace KladrTask.WebUI.Models
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Пожалуйста, введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите фамилию")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Пожалуйста, введите дату рождения")]
        [Display(Name = "дата рождения")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Пожалуйста, выберете регион")]
        public string RegionCode { get; set; }

        [Required(ErrorMessage = "Пожалуйста, выберете населенный пункт")]
        public string LocalityCode { get; set; }

        [Required(ErrorMessage = "Пожалуйста, выберете улицу")]
        public string RoadCode { get; set; }

        [Required(ErrorMessage = "Пожалуйста, выберете дом")]
        public string HouseCode { get; set; }
    }
}