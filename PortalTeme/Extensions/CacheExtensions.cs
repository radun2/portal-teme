﻿using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores.Serialization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PortalTeme.Extensions.CacheExtensions {
    internal static class CacheExtensions {
        internal static readonly JsonSerializerSettings Settings;

        static CacheExtensions() {
            Settings = new JsonSerializerSettings();
            Settings.Converters.Add(new ClaimConverter());
        }

        public static async Task<IEnumerable<Claim>> GetClaimsAsync(this IDistributedCache cache, string token) {
            var bytes = await cache.GetAsync(token.Sha256()).ConfigureAwait(false);

            if (bytes == null) {
                return null;
            }

            var json = Encoding.UTF8.GetString(bytes);
            return JsonConvert.DeserializeObject<IEnumerable<Claim>>(json, Settings);
        }

        public static async Task SetClaimsAsync(this IDistributedCache cache, string token, IEnumerable<Claim> claims, TimeSpan duration, ILogger logger) {
            var expClaim = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Expiration);
            if (expClaim == null) {
                logger.LogWarning("No exp claim found on introspection response, can't cache.");
                return;
            }

            var now = DateTimeOffset.UtcNow;
            var expiration = int.Parse(expClaim.Value).ToDateTimeOffsetFromEpoch();
            logger.LogDebug("Token will expire in {expiration}", expiration);


            if (expiration <= now) {
                return;
            }

            // if the lifetime of the token is shorter than the duration, use the remaining token lifetime
            DateTimeOffset absoluteLifetime;
            if (expiration <= now.Add(duration)) {
                absoluteLifetime = expiration;
            } else {
                absoluteLifetime = now.Add(duration);
            }

            var json = JsonConvert.SerializeObject(claims, Settings);
            var bytes = Encoding.UTF8.GetBytes(json);

            logger.LogDebug("Setting cache item expiration to {expiration}", absoluteLifetime);
            await cache.SetAsync(token.Sha256(), bytes, new DistributedCacheEntryOptions { AbsoluteExpiration = absoluteLifetime }).ConfigureAwait(false);
        }
    }
}
