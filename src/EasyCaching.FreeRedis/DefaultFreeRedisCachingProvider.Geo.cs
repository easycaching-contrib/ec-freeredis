namespace EasyCaching.FreeRedis
{
    using EasyCaching.Core;
    using global::FreeRedis;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public partial class DefaultFreeRedisCachingProvider : IRedisCachingProvider
    {
        public long GeoAdd(string cacheKey, List<(double longitude, double latitude, string member)> values)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(values, nameof(values));

            var list = new List<GeoMember>();

            foreach (var (longitude, latitude, member) in values)
            {
                list.Add(new GeoMember((decimal)longitude, (decimal)latitude, member));
            }

            var res = _cache.GeoAdd(cacheKey, list.ToArray());
            return res;
        }

        public async Task<long> GeoAddAsync(string cacheKey, List<(double longitude, double latitude, string member)> values)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(values, nameof(values));

            var list = new List<GeoMember>();

            foreach (var (longitude, latitude, member) in values)
            {
                list.Add(new GeoMember((decimal)longitude, (decimal)latitude, member));
            }

            var res = await _cache.GeoAddAsync(cacheKey, list.ToArray());
            return res;
        }

        public double? GeoDist(string cacheKey, string member1, string member2, string unit = "m")
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(member1, nameof(member1));
            ArgumentCheck.NotNullOrWhiteSpace(member2, nameof(member2));
            ArgumentCheck.NotNullOrWhiteSpace(unit, nameof(unit));

            var res = _cache.GeoDist(cacheKey, member1, member2, GetGeoUnit(unit));
            return (double?)res;
        }

        public async Task<double?> GeoDistAsync(string cacheKey, string member1, string member2, string unit = "m")
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullOrWhiteSpace(member1, nameof(member1));
            ArgumentCheck.NotNullOrWhiteSpace(member2, nameof(member2));
            ArgumentCheck.NotNullOrWhiteSpace(unit, nameof(unit));

            var res = await _cache.GeoDistAsync(cacheKey, member1, member2, GetGeoUnit(unit));
            return (double?)res;
        }

        public List<string> GeoHash(string cacheKey, List<string> members)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(members, nameof(members));

            var res = _cache.GeoHash(cacheKey, members.ToArray());
            return res.ToList();
        }

        public async Task<List<string>> GeoHashAsync(string cacheKey, List<string> members)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(members, nameof(members));

            var res = await _cache.GeoHashAsync(cacheKey, members.ToArray());
            return res.ToList();
        }

        public List<(decimal longitude, decimal latitude)?> GeoPos(string cacheKey, List<string> members)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(members, nameof(members));

            var res = _cache.GeoPos(cacheKey, members.ToArray());

            var ms = new List<(decimal longitude, decimal latitude)?>();
            foreach (var m in res)
            {
                ms.Add((m.longitude, m.latitude));
            }

            return ms;
        }

        public async Task<List<(decimal longitude, decimal latitude)?>> GeoPosAsync(string cacheKey, List<string> members)
        {
            ArgumentCheck.NotNullOrWhiteSpace(cacheKey, nameof(cacheKey));
            ArgumentCheck.NotNullAndCountGTZero(members, nameof(members));

            var res = await _cache.GeoPosAsync(cacheKey, members.ToArray());

            var ms = new List<(decimal longitude, decimal latitude)?>();
            foreach (var m in res)
            {
                ms.Add((m.longitude, m.latitude));
            }

            return ms;
        }

        private GeoUnit GetGeoUnit(string unit)
        {
            GeoUnit geoUnit;
            switch (unit)
            {
                case "km":
                    geoUnit = GeoUnit.km;
                    break;
                case "ft":
                    geoUnit = GeoUnit.ft;
                    break;
                case "mi":
                    geoUnit = GeoUnit.mi;
                    break;
                default:
                    geoUnit = GeoUnit.m;
                    break;
            }
            return geoUnit;
        }
    }
}
