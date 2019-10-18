﻿using System.Data.Entity;

using Liyanjie.FakeMQ.Sample.Console.Net.Infrastructure.EntityConfigurations;
using Liyanjie.FakeMQ.Sample.Console.Net.Models;

namespace Liyanjie.FakeMQ.Sample.Console.Net.Infrastructure
{
    public class SqlCeContext : System.Data.Entity.DbContext
    {
        public SqlCeContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            Database.SetInitializer(new CreateDatabaseIfNotExists<SqlCeContext>());
        }

        public IDbSet<FakeMQEvent> FakeMQEvents { get; set; }
        public IDbSet<FakeMQProcess> FakeMQProcesses { get; set; }

        public IDbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new FakeMQEventConfiguration());
            modelBuilder.Configurations.Add(new FakeMQProcessConfiguration());
            modelBuilder.Configurations.Add(new MessageConfiguration());
        }
    }
}
