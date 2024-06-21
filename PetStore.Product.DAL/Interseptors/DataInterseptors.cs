﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PetStore.Products.Domain.Interfaces;

namespace PetStore.Products.DAL.Interceptors
{
    public class DataInterceptors : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
            CancellationToken token = new CancellationToken())
        { 
            var dbContext = eventData.Context;
            if(dbContext == null)
            {
                return base.SavingChangesAsync(eventData,result,token);
            }
            var list = dbContext.ChangeTracker.Entries<IAuditable>()
                .Where(p => p.State == EntityState.Added || p.State == EntityState.Modified);
            foreach( var entry in list )
            {
                if(entry.State == EntityState.Added)
                {
                    entry.Property(p=>p.CreatedAt).CurrentValue = DateTime.UtcNow;
                }
                if(entry.State == EntityState.Modified)
                {
                    entry.Property(p=>p.UpdatedAt).CurrentValue = DateTime.UtcNow;
                }
            }
            return base.SavingChangesAsync(eventData,result, token);
        }
    }
}
