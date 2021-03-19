using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Api.Models
{
    public class AppContext:DbContext
    {
        public AppContext():base("AppContext")
        {

        }
        public DbSet<Pessoa> Pessoas { get; set; }
    }
}