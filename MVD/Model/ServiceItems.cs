//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVD.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class ServiceItems
    {
        public int IDServiceItems { get; set; }
        public int IDEntrancePass { get; set; }
        public int IDEmployee { get; set; }
        public int IDVehicle { get; set; }
        public int IDWeapon { get; set; }
        public int IDUniform { get; set; }
        public int IDTraining { get; set; }
    
        public virtual Employee Employee { get; set; }
        public virtual EntrancePass EntrancePass { get; set; }
        public virtual Training Training { get; set; }
        public virtual Unifrorm Unifrorm { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public virtual Weapon Weapon { get; set; }
    }
}
