﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExpTrackerAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ExpenseTrackerEntities : DbContext
    {
        public ExpenseTrackerEntities()
            : base("name=ExpenseTrackerEntities")
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled= false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<category> categories { get; set; }
        public virtual DbSet<expense> expenses { get; set; }
        public virtual DbSet<ExpenseLimit> ExpenseLimits { get; set; }

        public System.Data.Entity.DbSet<ExpTrackerAPI.Models.home> homes { get; set; }
    }
}