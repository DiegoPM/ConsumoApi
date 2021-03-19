using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models
{
    [Table("Pessoas")]
    public class Pessoa
    {
        [Key]
        public int Id { get; set; }
        [StringLength(45)]
        public string Name { get; set; }
        [StringLength(45)]
        public string Email { get; set; }
        [StringLength(20)]
        public string Username { get; set; }
        [StringLength(20)]
        public string Password { get; set; }
    }
}