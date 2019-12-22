﻿using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Liyanjie.FakeMQ
{
    /// <summary>
    /// 
    /// </summary>
    public class FakeMQProcessStore : IFakeMQProcessStore
    {
        readonly IServiceProvider serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public FakeMQProcessStore(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <inheritdoc />
        public async Task AddAsync(FakeMQProcess process)
        {
            using var context = FakeMQContext.GetContext(serviceProvider);
            if (await context.FakeMQProcesses.AnyAsync(_ => _.HandlerType == process.HandlerType))
                return;
            context.FakeMQProcesses.Add(process);
            await context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public void Add(FakeMQProcess process)
        {
            using var context = FakeMQContext.GetContext(serviceProvider);
            if (context.FakeMQProcesses.Any(_ => _.HandlerType == process.HandlerType))
                return;
            context.FakeMQProcesses.Add(process);
            context.SaveChanges();
        }

        /// <inheritdoc />
        public async Task<FakeMQProcess> GetAsync(string handlerType)
        {
            using var context = FakeMQContext.GetContext(serviceProvider);
            return await context.FakeMQProcesses
                .AsNoTracking()
                .FirstOrDefaultAsync(_ => _.HandlerType == handlerType);
        }

        /// <inheritdoc />
        public FakeMQProcess Get(string handlerType)
        {
            using var context = FakeMQContext.GetContext(serviceProvider);
            return context.FakeMQProcesses
                .AsNoTracking()
                .FirstOrDefault(_ => _.HandlerType == handlerType);
        }

        /// <inheritdoc />
        public async Task UpdateAsync(string handlerType, DateTimeOffset handleTime)
        {
            using var context = FakeMQContext.GetContext(serviceProvider);
            var item = await context.FakeMQProcesses
                .AsTracking()
                .FirstOrDefaultAsync(_ => _.HandlerType == handlerType);
            if (item == null)
                return;
            item.LastHandleTime = handleTime;
            await context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public void Update(string handlerType, DateTimeOffset handleTime)
        {
            using var context = FakeMQContext.GetContext(serviceProvider);
            var item = context.FakeMQProcesses
                .AsTracking()
                .FirstOrDefault(_ => _.HandlerType == handlerType);
            if (item == null)
                return;
            item.LastHandleTime = handleTime;
            context.SaveChanges();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(string handlerType)
        {
            using var context = FakeMQContext.GetContext(serviceProvider);
            var item = await context.FakeMQProcesses
                .AsTracking()
                .FirstOrDefaultAsync(_ => _.HandlerType == handlerType);
            if (item == null)
                return;
            context.FakeMQProcesses.Remove(item);
            await context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public void Delete(string handlerType)
        {
            using var context = FakeMQContext.GetContext(serviceProvider);
            var item = context.FakeMQProcesses
                .AsTracking()
                .FirstOrDefault(_ => _.HandlerType == handlerType);
            if (item == null)
                return;
            context.FakeMQProcesses.Remove(item);
            context.SaveChanges();
        }
    }
}
