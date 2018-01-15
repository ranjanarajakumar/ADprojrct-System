namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Requisition Record")]
    public partial class Requisition_Record
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Requisition_Record()
        {
            Requisition_Details = new HashSet<Requisition_Details>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int requisitionNo { get; set; }

        [Required]
        [StringLength(50)]
        public string deptCode { get; set; }

        [Required]
        [StringLength(50)]
        public string requesterID { get; set; }

        [StringLength(50)]
        public string approvingStaffID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? approveDate { get; set; }

        [Required]
        [StringLength(50)]
        public string status { get; set; }

        [Column(TypeName = "date")]
        public DateTime requestDate { get; set; }

        public virtual Department Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition_Details> Requisition_Details { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}