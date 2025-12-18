using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace OrenburgCommunElectroNetwork.Models
{
    public class CalendarDay
    {
        public DateTime Date { get; set; }
        public int DayNumber { get; set; }
        public bool IsCurrentMonth { get; set; }
        public bool IsToday { get; set; }
        public bool HasNote { get; set; }
        public string NoteText { get; set; }
        public Color NoteColor { get; set; }
        public Brush NoteColorBrush => new SolidColorBrush(NoteColor);
    }
}