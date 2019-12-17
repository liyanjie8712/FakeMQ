﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Liyanjie.FakeMQ.Sample.Console.Net.Infrastructure
{
    public class FakeMQEventStore : IFakeMQEventStore
    {
        readonly string dbConnectionString;
        public FakeMQEventStore(string dbConnectionString)
        {
            this.dbConnectionString = dbConnectionString ?? throw new ArgumentNullException(nameof(dbConnectionString));
        }

        public async Task AddAsync(FakeMQEvent @event)
        {
            using var context = new DataContext(dbConnectionString);
            context.FakeMQEvents.Add(@event);
            await context.SaveChangesAsync();
        }
        public void Add(FakeMQEvent @event)
        {
            using var context = new DataContext(dbConnectionString);
            context.FakeMQEvents.Add(@event);
            context.SaveChanges();
        }
        public async Task<IEnumerable<FakeMQEvent>> GetAsync(string type, long startTimestamp, long endTimestamp)
        {
            using var context = new DataContext(dbConnectionString);
            return await context.FakeMQEvents.AsNoTracking()
                .Where(_ => _.Type == type && _.Timestamp > startTimestamp && _.Timestamp < endTimestamp)
                .OrderBy(_ => _.Timestamp)
                .ToListAsync();
        }
    }
}
