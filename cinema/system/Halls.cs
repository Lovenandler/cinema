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
    
    public partial class Halls
    {
        public Halls()
        {
            this.Seats = new HashSet<Seats>();
        }
    
        public int Number_Hall { get; set; }
        public string Category_Hall { get; set; }
    
        public virtual ICollection<Seats> Seats { get; set; }
    }
}
