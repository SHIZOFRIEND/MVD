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
    
    public partial class Investigation
    {
        public int IDInvestigation { get; set; }
        public int IDInvestigationStatus { get; set; }
        public int IDInvestigationType { get; set; }
        public int IDCase { get; set; }
        public int IDEmployee { get; set; }
        public string Title { get; set; }
        public string DescriptionInvestigation { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
    
        public virtual Cases Cases { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual InvestigationStatus InvestigationStatus { get; set; }
        public virtual InvestigationType InvestigationType { get; set; }
    }
}
