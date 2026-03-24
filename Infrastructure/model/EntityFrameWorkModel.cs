using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace api_para_banco.Infrastructure.model
{
    public class EntityFrameWorkModel : DbContext
    {
        public EntityFrameWorkModel(DbContextOptions options) : base(options) { }
        
        //essas declarações servem para o EF intrepretar essas classes como tabelas no banco de dados e criar as relações entre elas
        public DbSet<ContaCorrente> ContaCorrente { get; set; }
        public DbSet<ContaPoupanca> ContaPoupanca { get; set; }
    }
}
