using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace KladrTask.WebUI.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }
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

        public string RoadCode { get; set; }

        [Required(ErrorMessage = "Пожалуйста, ввеите номер дома")]
        public string HouseNumber { get; set; }
        public string Housing { get; set; }
        public string ApartamentNumber { get; set; }

        [Required(ErrorMessage = "Пожалуйста, выберете индекс")]
        public string Index { get; set; }
    }
}