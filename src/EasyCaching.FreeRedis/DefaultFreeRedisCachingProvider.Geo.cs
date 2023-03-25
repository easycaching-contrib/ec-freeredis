﻿namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using global::FreeRedis;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : IRedisCachingProvider
    {
        public long GeoAdd(string cacheKey, List<(double longitude, double latitude, string member)> values)
        {
            throw new NotImplementedException();
        }

        public async Task<long> GeoAddAsync(string cacheKey, List<(double longitude, double latitude, string member)> values)
        {
            throw new NotImplementedException();
        }

        public double? GeoDist(string cacheKey, string member1, string member2, string unit = "m")
        {
            throw new NotImplementedException();
        }

        public async Task<double?> GeoDistAsync(string cacheKey, string member1, string member2, string unit = "m")
        {
            throw new NotImplementedException();
        }

        public List<string> GeoHash(string cacheKey, List<string> members)
        {
            throw new NotImplementedException();
        }

        public async Task<List<string>> GeoHashAsync(string cacheKey, List<string> members)
        {
            throw new NotImplementedException();
        }

        public List<(decimal longitude, decimal latitude)?> GeoPos(string cacheKey, List<string> members)
        {
            throw new NotImplementedException();
        }

        public async Task<List<(decimal longitude, decimal latitude)?>> GeoPosAsync(string cacheKey, List<string> members)
        {
            throw new NotImplementedException();
        }

        private GeoUnit GetGeoUnit(string unit)
        {
            throw new NotImplementedException();
        }
    }
}