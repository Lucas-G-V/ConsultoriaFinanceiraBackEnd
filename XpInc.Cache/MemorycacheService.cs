using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpInc.Cache
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private string _redisConnectionString;
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(1);
        private ConnectionMultiplexer _redisConnection;

        private DateTime? tempoDeEsperaParaFazerSolicitacoes = null;
        private int tentativasConsecutivasErradas = 0;
        public MemoryCacheService(string connectionString)
        {
            _redisConnectionString = connectionString;
        }

        private async Task<ConnectionMultiplexer> GetRedisConnection()
        {
            if (_redisConnection != null && _redisConnection.IsConnected)
            {
                return _redisConnection;
            }

            if (tempoDeEsperaParaFazerSolicitacoes == null || DateTime.Now >= tempoDeEsperaParaFazerSolicitacoes)
            {
                var connectionTask = TryConnectAsync();
                var timeoutTask = Task.Delay(TimeSpan.FromMilliseconds(200));
                await Task.WhenAny(connectionTask, timeoutTask);

                if (!connectionTask.IsCompleted)
                {
                    tentativasConsecutivasErradas++;
                    if (tentativasConsecutivasErradas >= 3)
                    {
                        tempoDeEsperaParaFazerSolicitacoes = DateTime.Now.AddMinutes(0.5);
                        tentativasConsecutivasErradas = 0;
                    }
                    throw new TimeoutException("Failed to establish connection to Redis within the timeout period.");
                }

                return await connectionTask;
            }
            throw new Exception("Limite de tentativas de se conectar ao Redis Excedido");

        }

        private async Task<ConnectionMultiplexer> TryConnectAsync()
        {
            await _connectionLock.WaitAsync();
            try
            {
                if (_redisConnection == null || !_redisConnection.IsConnected)
                {
                    _redisConnection?.Dispose();
                    _redisConnection = await ConnectionMultiplexer.ConnectAsync(_redisConnectionString + ",allowAdmin=true");
                    var server = _redisConnection.GetServer(_redisConnectionString, 6379);
                    await server.FlushDatabaseAsync(0);
                }

            }
            finally
            {
                _connectionLock.Release();
            }
            return _redisConnection;
        }

        public async Task AddMemoryCache<T>(string id, T dados)
        {
            try
            {
                var redis = await GetRedisConnection();
                if (redis.IsConnected)
                {
                    var db = redis.GetDatabase();
                    string json = JsonConvert.SerializeObject(dados);
                    json = CriptografiaRedis.Encrypt(json);
                    id = CriptografiaRedis.Encrypt(id);
                    await db.StringSetAsync(id, json, TimeSpan.FromDays(1));
                }
            }
            catch (Exception) { }
        }

        public async Task<TData?> GetById<TData>(string id)
        {
            try
            {
                var redis = await GetRedisConnection();
                if (redis.IsConnected)
                {
                    var db = redis.GetDatabase();
                    id = CriptografiaRedis.Encrypt(id);
                    string jsonRetornado = await db.StringGetAsync(id);
                    if (jsonRetornado != null)
                    {
                        jsonRetornado = CriptografiaRedis.Decrypt(jsonRetornado);
                        return JsonConvert.DeserializeObject<TData>(jsonRetornado);
                    }
                }
            }
            catch (Exception) { }

            return default;
        }

        public async Task DeleteMemoryCache(string id)
        {
            try
            {
                var redis = await GetRedisConnection();
                if (redis.IsConnected)
                {
                    var db = redis.GetDatabase();
                    id = CriptografiaRedis.Encrypt(id);
                    await db.KeyDeleteAsync(id);
                }
            }
            catch (Exception) { }
        }
    }
}
