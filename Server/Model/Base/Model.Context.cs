﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Server.Model.Base
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class workload_baseEntities : DbContext
    {
        public workload_baseEntities()
            : base("name=workload_baseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<curriculum> curricula { get; set; }
        public virtual DbSet<discipline> disciplines { get; set; }
        public virtual DbSet<finance_form_types> finance_form_types { get; set; }
        public virtual DbSet<group> groups { get; set; }
        public virtual DbSet<profession> professions { get; set; }
        public virtual DbSet<specialization> specializations { get; set; }
        public virtual DbSet<teachers_schedule> teachers_schedule { get; set; }
        public virtual DbSet<teachers_workload> teachers_workload { get; set; }
        public virtual DbSet<user> users { get; set; }
    }
}
