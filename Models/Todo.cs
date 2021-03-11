using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApiAssignment.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? TaskTitle { get; set; }
        [Required]
        public DateTime? DateTime { get; set; }
    }
}
