using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBotInt.Repositories.Models
{
    public class ThemeOfTreatmentModel
    {
        public Guid SessionId { get; set; }
        public string Theme { get; set; }
        public string SubTheme { get; set; }
        public string Comment { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ThemeId { get; set; }
        public Guid? SubThemeId { get; set; }
    }
}
