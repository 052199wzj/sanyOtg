using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace iPlant.FMS.Models
{
    [Table("mcs_loginfo")]
    public class mcs_loginfo
    {
        [Key]
        [Column(TypeName = "int(11)")]
        public int ID { get; set; }
        [Required]
        [StringLength(45)]
        public string CustomerName { get; set; }
        [Required]
        [StringLength(45)]
        public string LineName { get; set; }
        [Required]
        [StringLength(45)]
        public string ProductNo { get; set; }
        [Required]
        [StringLength(45)]
        public string PartNo { get; set; }
        [Required]
        [StringLength(45)]
        public string VersionNo { get; set; }
        [Required]
        [StringLength(255)]
        public string FileName { get; set; }
        [Required]
        [StringLength(255)]
        public string FilePath { get; set; }
        [Required]
        [StringLength(45)]
        public string FileType { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
        [Required]
        [StringLength(45)]
        public string CreateTimeStr { get; set; }
        [Column(TypeName = "int(11)")]
        public int BOPID { get; set; }
        [Required]
        [StringLength(4096)]
        public string BOMID { get; set; }
        [Required]
        [StringLength(45)]
        public string SystemType { get; set; }
        [Required]
        [StringLength(500)]
        public string Info { get; set; }
        [Column(TypeName = "int(11)")]
        public int StepNo { get; set; }
        [Required]
        [StringLength(100)]
        public string ProcessName { get; set; }
    }
}
