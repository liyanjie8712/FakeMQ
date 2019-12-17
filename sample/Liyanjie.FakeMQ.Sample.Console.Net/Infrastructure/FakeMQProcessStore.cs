﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Liyanjie.FakeMQ.Sample.Console.Net.Infrastructure
{
    public class FakeMQProcessStore : IFakeMQProcessStore
    {
        readonly string dbConnectionString;
        public FakeMQProcessStore(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString ?? throw new ArgumentNullException(nameof(dbConnectionString));
        }

        public async Task AddAsync(FakeMQProcess process)
        {
            using var context = new DataContext(dbConnectionString);
            if (await context.FakeMQProcesses.AnyAsync(_ => _.Subscription == process.Subscription))
                return;
            context.FakeMQProcesses.Add(process);
            await context.SaveChangesAsync();
        }
        public void Add(FakeMQProcess process)
        {
            using var context = new DataContext(dbConnectionString);
            if (context.FakeMQProcesses.Any(_ => _.Subscription == process.Subscription))
                return;
            context.FakeMQProcesses.Add(process);
            context.SaveChanges();
        }
        public async Task<FakeMQProcess> GetAsync(string subscription)
        {
            using var context = new DataContext(dbConnectionString);
            return await context.FakeMQProcesses.AsNoTracking()
                .SingleOrDefaultAsync(_ => _.Subscription == subscription);
        }
        public async Task UpdateAsync(string subscription, long timestamp)
        {
            using var context = new DataContext(dbConnectionString);
            var item = await context.FakeMQProcesses.SingleOrDefaultAsync(_ => _.Subscription == subscription);
            if (item == null)
                return;
            item.Timestamp = timestamp;
            await context.SaveChangesAsync();
        }
        public async Task DeleteAsync(string subscription)
        {
            using var context = new DataContext(dbConnectionString);
            var item = await context.FakeMQProcesses.SingleOrDefaultAsync(_ => _.Subscription == subscription);
            if (item == null)
                return;
            context.FakeMQProcesses.Remove(item);
            await context.SaveChangesAsync();
        }

        public void Delete(string subscription)
        {
            using var context = new DataContext(dbConnectionString);
            var item = context.FakeMQProcesses.SingleOrDefault(_ => _.Subscription == subscription);
            if (item == null)
                return;
            context.FakeMQProcesses.Remove(item);
            context.SaveChanges();
        }
    }
}
