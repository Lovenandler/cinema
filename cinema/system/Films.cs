//------------------------------------------------------------------------------
// <auto-generated>
//    Этот код был создан из шаблона.
//
//    Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//    Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace cinema.system
{
    using System;
    using System.Collections.Generic;
    
    public partial class Films
    {
        public Films()
        {
            this.Schedule = new HashSet<Schedule>();
        }
    
        public int ID_Film { get; set; }
        public string Name_Film { get; set; }
        public string Description_Film { get; set; }
        public string In_Roles { get; set; }
        public int Age_Restrictions { get; set; }
        public System.DateTime Year_Of_Release { get; set; }
        public Nullable<System.TimeSpan> Duration { get; set; }
        public int ID_Producer { get; set; }
        public int ID_Genre { get; set; }
        public Nullable<double> Rating_Of_The_Film { get; set; }
        public int ID_country { get; set; }
        public string Language_Film { get; set; }
        public byte[] Cover { get; set; }
    
        public virtual Country Country { get; set; }
        public virtual Genre Genre { get; set; }
        public virtual ICollection<Schedule> Schedule { get; set; }
    }
}