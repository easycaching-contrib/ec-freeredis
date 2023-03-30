namespace EasyCaching.FreeRedis
{
    using global::FreeRedis;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    public class EasyCachingFreeRedisClient : RedisClient
    {
        public NodesServerManagerProvider NodesServerManager { get; set; }

        private readonly string _name;

        public string Name { get { return this._name; } }

        public EasyCachingFreeRedisClient(string name, ConnectionStringBuilder connectionString, params ConnectionStringBuilder[] slaveConnectionStrings)
            : base(connectionString, slaveConnectionStrings)
        {
            this._name = name;
            NodesServerManager = new NodesServerManagerProvider(this, Adapter.UseType == UseType.Cluster);
        }

        public EasyCachingFreeRedisClient(string name, ConnectionStringBuilder[] clusterConnectionStrings)
            : base(clusterConnectionStrings)
        {
            this._name = name;
            NodesServerManager = new NodesServerManagerProvider(this, Adapter.UseType == UseType.Cluster);
        }

        public EasyCachingFreeRedisClient(string name, ConnectionStringBuilder[] connectionStrings, Func<string, string> redirectRule)
           : base(connectionStrings, redirectRule)
        {
            this._name = name;
            NodesServerManager = new NodesServerManagerProvider(this, Adapter.UseType == UseType.Cluster);
        }

        public EasyCachingFreeRedisClient(string name, ConnectionStringBuilder sentinelConnectionString, string[] sentinels, bool rw_splitting)
          : base(sentinelConnectionString, sentinels, rw_splitting)
        {
            this._name = name;
            NodesServerManager = new NodesServerManagerProvider(this, Adapter.UseType == UseType.Cluster);
        }

        public partial class NodesServerManagerProvider
        {
            private RedisClient _client;
            private bool _isCluster;

            public NodesServerManagerProvider(RedisClient client, bool isCluster = false)
            {
                _client = client;
                _isCluster = isCluster;
            }

            public List<long> DbSize() => CallCommandFunc(cli => cli.DbSize());

            public Task<List<long>> DbSizeAsync() => CallCommandFuncAsync(cli => cli.DbSize());

            public List<object> FlushDb(bool isasync = false) => CallCommandFunc(cli => cli.Call(new CommandPacket("FLUSHDB").InputIf(isasync, "ASYNC")));

            public Task<List<object>> FlushDbAsync(bool isasync = false) => CallCommandFuncAsync(cli => cli.Call(new CommandPacket("FLUSHDB", null).InputIf(isasync, "ASYNC")));

            public List<string> Keys(string pattern, int pageSize)
            {
                var groupKeys = CallCommandFunc(cli =>
                {
                    var masterKeys = new List<string>();
                    long nextCursor = 0;
                    do
                    {
                        var scanResult = cli.Scan(nextCursor, pattern, pageSize, null);
                        nextCursor = scanResult.cursor;
                        var items = scanResult.items;
                        masterKeys.AddRange(items);
                    }
                    while (nextCursor != 0);

                    return masterKeys;
                });

                var allKeys = new List<string>();
                foreach (var keys in groupKeys)
                {
                    allKeys.AddRange(keys);
                }

                return allKeys;
            }

            /// <summary>
            /// Call command by func.
            /// </summary>
            /// <returns></returns>
            private List<T> CallCommandFunc<T>(Func<RedisClient, T> func)
            {
                List<T> result = new List<T>();

                object nodes = null;
                if (_isCluster)
                    nodes = _client.Call(new CommandPacket("CLUSTER").Input("NODES"));

                return InvokeFunc(nodes, func);
            }

            /// <summary>
            /// Call command by func async.
            /// </summary>
            /// <returns></returns>
            private async Task<List<T>> CallCommandFuncAsync<T>(Func<RedisClient, T> func)
            {
                List<T> result = new List<T>();

                object nodes = null;
                if (_isCluster)
                    nodes = await _client.CallAsync(new CommandPacket("CLUSTER").Input("NODES"));

                return InvokeFunc(nodes, func);
            }

            /// <summary>
            /// The cluster node client invok func.
            /// </summary>
            /// <returns></returns>
            private List<T> InvokeFunc<T>(object nodes, Func<RedisClient, T> func)
            {
                List<T> result = new List<T>();

                if (nodes != null)
                {
                    var hosts = ParseClusterNodeHosts(nodes);
                    foreach (var host in hosts)
                    {
                        using (var cli = new RedisClient(host))
                        {
                            result.Add(func.Invoke(cli));
                        }
                    }
                }
                else
                {
                    result.Add(func.Invoke(_client));
                }
                return result;
            }

            /// <summary>
            /// Parsing cluster nodes host.
            /// </summary>
            /// <param name="nodes"></param>
            /// <returns></returns>
            /// <exception cref="Exception"></exception>
            private List<string> ParseClusterNodeHosts(object nodes)
            {
                if (nodes == null) throw new Exception("Cluster nodes is null");
                var host = "127.0.0.1";
                var masterHosts = new List<string>();
                var cnodes = nodes.ToString().Split('\n');
                foreach (var cnode in cnodes)
                {
                    if (string.IsNullOrEmpty(cnode)) continue;
                    var dt = cnode.Trim().Split(' ');
                    if (dt.Length < 9) continue;
                    if (!dt[2].StartsWith("master") && !dt[2].EndsWith("master")) continue;
                    if (dt[7] != "connected") continue;

                    var endpoint = dt[1];
                    var at40 = endpoint.IndexOf('@');
                    if (at40 != -1) endpoint = endpoint.Remove(at40);

                    if (endpoint.StartsWith("127.0.0.1"))
                        endpoint = $"{host}:{endpoint.Substring(10)}";
                    else if (endpoint.StartsWith("localhost", StringComparison.CurrentCultureIgnoreCase))
                        endpoint = $"{host}:{endpoint.Substring(10)}";
                    masterHosts.Add($"{endpoint},defaultDatabase=1,poolsize=1");
                }
                return masterHosts;
            }
        }
    }
}
