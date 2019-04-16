﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookiesDemo.TicketStore
{
    /// <summary>
    /// 用于将Cookie存放到Memory中
    /// </summary>
    public class MemoryCacheTicketStore : ITicketStore
    {
        private const string KeyPrefix = "CSS-";
        private IMemoryCache _cache;

        public MemoryCacheTicketStore()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }
        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.FromResult(0);
        }

        public Task RenewAsync(string key, AuthenticationTicket ticket)
        {
            var options = new MemoryCacheEntryOptions();
            var expiresUtc = ticket.Properties.ExpiresUtc;
            if (expiresUtc.HasValue)
            {
                options.SetAbsoluteExpiration(expiresUtc.Value);
            }
            options.SetSlidingExpiration(TimeSpan.FromHours(1));
            _cache.Set(key, ticket, options);
            return Task.FromResult(0);
        }

        public Task<AuthenticationTicket> RetrieveAsync(string key)
        {
            _cache.TryGetValue(key, out AuthenticationTicket ticket);
            return Task.FromResult(ticket);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        public async Task<string> StoreAsync(AuthenticationTicket ticket)
        {
            var key = KeyPrefix + Guid.NewGuid().ToString("N");
            await RenewAsync(key, ticket);
            return key;
        }
    }
}
