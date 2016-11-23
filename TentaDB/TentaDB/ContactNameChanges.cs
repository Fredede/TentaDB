namespace TentaDB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ContactNameChanges
    {
        [Key]
        public int ContactNameChangeID { get; set; }

        [Required]
        [StringLength(5)]
        public string CustomerID { get; set; }

        [Required]
        [StringLength(30)]
        public string OldName { get; set; }

        [Required]
        [StringLength(30)]
        public string NewName { get; set; }

        public DateTime ChangedDate { get; set; }
    }
}
