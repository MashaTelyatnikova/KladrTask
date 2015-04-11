using KladrTask.Domain.Entities;

namespace KladrTask.WebUI.Models
{
    public class LetterViewModel
    {
        public string Receiver { get; set; }
        public string Address { get; set; }
        public string Index { get; set; }
        public string Text { get; set; }
    }
}
