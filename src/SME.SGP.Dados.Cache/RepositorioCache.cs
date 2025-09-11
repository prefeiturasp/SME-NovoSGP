using Newtonsoft.Json;
using SME.SGP.Dados.Cache;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioCache : IRepositorioCache
    {
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly IServicoMensageriaLogs servicoMensageriaLogs;
        private readonly IMetricasCache metricasCache;

        protected string NomeServicoCache { get; set; }

        protected virtual string ObterValor(string nomeChave)
            => throw new NotImplementedException($"Método ObterValor do serviço {NomeServicoCache} não implementado");

        protected virtual Task RemoverValor(string nomeChave)
            => throw new NotImplementedException($"Método RemoverValor do serviço {NomeServicoCache} não implementado");

        protected virtual Task SalvarValor(string nomeChave, string valor, int minutosParaExpirar)
            => throw new NotImplementedException($"Método SalvarValor do serviço {NomeServicoCache} não implementado");

        public RepositorioCache(IServicoTelemetria servicoTelemetria, IServicoMensageriaLogs servicoMensageriaLogs, IMetricasCache metricasCache)
        {
            this.servicoTelemetria = servicoTelemetria ?? throw new ArgumentNullException(nameof(servicoTelemetria));
            this.servicoMensageriaLogs = servicoMensageriaLogs ?? throw new ArgumentNullException(nameof(servicoMensageriaLogs));
            this.metricasCache = metricasCache ?? throw new ArgumentNullException(nameof(metricasCache));
        }

        public string Obter(string nomeChave, string telemetriaNome, bool utilizarGZip = false)
        {
            try
            {
                var param = new
                {
                    NomeChave = nomeChave,
                    UtilizarGZip = utilizarGZip
                };
                
                var cacheParaRetorno = servicoTelemetria.RegistrarComRetorno<string>(() => ObterValor(nomeChave), 
                    NomeServicoCache, $"{NomeServicoCache}: {telemetriaNome}", "", param.ToString());

                if (utilizarGZip)
                    cacheParaRetorno = UtilGZip.Descomprimir(Convert.FromBase64String(cacheParaRetorno));

                if (!string.IsNullOrEmpty(cacheParaRetorno))
                    metricasCache.Hit(nomeChave);
                else
                    metricasCache.Miss(nomeChave);

                return cacheParaRetorno;    
            }
            catch (Exception e)
            {
                metricasCache.Fail(nomeChave);

                var mensagem = new LogMensagem($"Erro ao obter cache - {NomeServicoCache}",
                    LogNivel.Alerta.ToString(),
                    LogContexto.Cache.ToString(),
                    $"Nome chave: {nomeChave}",
                    "SGP",
                    e.StackTrace,
                    e.InnerException?.Message,
                    e.InnerException?.ToString());

                servicoMensageriaLogs.Publicar(mensagem, RotasRabbitLogs.RotaLogs, ExchangeSgpRabbit.SgpLogs, "PublicarFilaLog").Wait();
                
                return string.Empty;
            }
        }
        
        public string Obter(string nomeChave, bool utilizarGZip = false)
        {
            return Obter(nomeChave, $"{NomeServicoCache} Obter", utilizarGZip);
        }        

        public async Task<string> ObterAsync(string nomeChave, string telemetriaNome, bool utilizarGZip = false)
        {
            try
            {
                var param = new
                {
                    NomeChave = nomeChave,
                    UtilizarGZip = utilizarGZip
                };
                    
                var stringCache = servicoTelemetria.RegistrarComRetorno<string>(() => ObterValor(nomeChave),
                    NomeServicoCache, $"{NomeServicoCache}: {telemetriaNome}", "", param.ToString());

                if (string.IsNullOrWhiteSpace(stringCache))
                {
                    metricasCache.Miss(nomeChave);
                    return await Task.FromResult(string.Empty);
                }
                
                if (utilizarGZip)
                    stringCache = UtilGZip.Descomprimir(Convert.FromBase64String(stringCache));

                metricasCache.Hit(nomeChave);
                return stringCache;
            }
            catch (Exception e)
            {
                metricasCache.Fail(nomeChave);
                var mensagem = new LogMensagem($"Erro ao obter cache - {NomeServicoCache}",
                    LogNivel.Alerta.ToString(),
                    LogContexto.Cache.ToString(),
                    $"Nome chave: {nomeChave}",
                    "SGP",
                    e.StackTrace,
                    e.InnerException?.Message,
                    e.InnerException?.ToString());

                await servicoMensageriaLogs.Publicar(mensagem, RotasRabbitLogs.RotaLogs, ExchangeSgpRabbit.SgpLogs, "PublicarFilaLog");
                    
                return string.Empty;
            }
        }

        public async Task<string> ObterAsync(string nomeChave, bool utilizarGZip = false)
        {
            return await ObterAsync(nomeChave, $"Obter async<string>", utilizarGZip);
        }

        public async Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, string telemetriaNome, int minutosParaExpirar = 720,
            bool utilizarGZip = false)
        {
            try
            {
                var param = new
                {
                    NomeChave = nomeChave,
                    MinutosParaExpirar = minutosParaExpirar,
                    UtilizarGZip = utilizarGZip
                };
            
                var stringCache = servicoTelemetria.RegistrarComRetorno<string>(() => ObterValor(nomeChave),
                    NomeServicoCache, $"{NomeServicoCache}: {telemetriaNome}", "", param.ToString());

                if (!string.IsNullOrWhiteSpace(stringCache))
                {
                    if (utilizarGZip)
                        stringCache = UtilGZip.Descomprimir(Convert.FromBase64String(stringCache));

                    metricasCache.Hit(nomeChave);
                    return JsonConvert.DeserializeObject<T>(stringCache);
                }

                metricasCache.Miss(nomeChave);
                var dados = await buscarDados();
                await SalvarAsync(nomeChave, JsonConvert.SerializeObject(dados), minutosParaExpirar, utilizarGZip);

                return dados;
            }
            catch (Exception e)
            {
                metricasCache.Fail(nomeChave);
                var mensagem = new LogMensagem($"Erro ao obter cache - {NomeServicoCache}",
                    LogNivel.Alerta.ToString(),
                    LogContexto.Cache.ToString(),
                    $"Nome chave: {nomeChave}",
                    "SGP",
                    e.StackTrace,
                    e.InnerException?.Message,
                    e.InnerException?.ToString());

                await servicoMensageriaLogs.Publicar(mensagem, RotasRabbitLogs.RotaLogs, ExchangeSgpRabbit.SgpLogs, "PublicarFilaLog");
                
                return await buscarDados();
            }
        }

        public async Task<T> ObterObjetoAsync<T>(string nomeChave, bool utilizarGZip = false) where T : new()
        {
            return await ObterObjetoAsync<T>(nomeChave, $"{NomeServicoCache} Obter objeto async<string>", utilizarGZip);
        }

        public async Task<T> ObterObjetoAsync<T>(string nomeChave, string telemetriaNome, bool utilizarGZip = false) where T : new() 
        {
            var param = new
            {
                NomeChave = nomeChave,
                UtilizarGZip = utilizarGZip
            };

            try
            {
                var stringCache = servicoTelemetria.RegistrarComRetorno<string>(() => ObterValor(nomeChave),
                                                                                NomeServicoCache,
                                                                                $"{NomeServicoCache}: {telemetriaNome}",
                                                                                "",
                                                                                param.ToString());

                if (string.IsNullOrWhiteSpace(stringCache))
                {
                    metricasCache.Miss(nomeChave);
                    return default;
                }

                if (utilizarGZip)
                    stringCache = UtilGZip.Descomprimir(Convert.FromBase64String(stringCache));

                metricasCache.Hit(nomeChave);
                return await Task.FromResult(JsonConvert.DeserializeObject<T>(stringCache));
            }
            catch (Exception e)
            {
                metricasCache.Fail(nomeChave);
                var mensagem = new LogMensagem($"Erro ao obter cache - {NomeServicoCache}",
                    LogNivel.Alerta.ToString(),
                    LogContexto.Cache.ToString(),
                    $"Nome chave: {nomeChave}",
                    "SGP",
                    e.StackTrace,
                    e.InnerException?.Message,
                    e.InnerException?.ToString());

                await servicoMensageriaLogs.Publicar(mensagem, RotasRabbitLogs.RotaLogs, ExchangeSgpRabbit.SgpLogs, "PublicarFilaLog");

                return default;
            }
        }

        public async Task<T> ObterAsync<T>(string nomeChave, Func<Task<T>> buscarDados, int minutosParaExpirar = 720,
            bool utilizarGZip = false)
        {
            return await ObterAsync(nomeChave, buscarDados, $"{NomeServicoCache} Obter async<T>", minutosParaExpirar, utilizarGZip);
        }

        public async Task RemoverAsync(string nomeChave)
        {
            var param = new
            {
                NomeChave = nomeChave
            };
            
            try
            {
                await servicoTelemetria.RegistrarAsync(async () => await RemoverValor(nomeChave),
                    NomeServicoCache, $"{NomeServicoCache} Remover async", "", param.ToString());

                metricasCache.Hit(nomeChave, "DELETE");
            }
            catch (Exception e)
            {
                metricasCache.Fail(nomeChave, "DELETE");
                var mensagem = new LogMensagem($"Erro ao remover cache - {NomeServicoCache}",
                    LogNivel.Alerta.ToString(),
                    LogContexto.Cache.ToString(),
                    $"Nome chave: {nomeChave}",
                    "SGP",
                    e.StackTrace,
                    e.InnerException?.Message,
                    e.InnerException?.ToString());
                
                await servicoMensageriaLogs.Publicar(mensagem, RotasRabbitLogs.RotaLogs, ExchangeSgpRabbit.SgpLogs, "PublicarFilaLog");
            }
        }

        public async Task SalvarAsync(string nomeChave, string valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            var param = new
            {
                NomeChave = nomeChave,
                MinutosParaExpirar = minutosParaExpirar,
                UtilizarGZip = utilizarGZip
            };
            
            try
            {
                if (!string.IsNullOrWhiteSpace(valor) && valor != "[]" && valor != "null")
                {
                    if (utilizarGZip)
                    {
                        var valorComprimido = UtilGZip.Comprimir(valor);
                        valor = Convert.ToBase64String(valorComprimido);
                    }

                    await servicoTelemetria.RegistrarAsync(async () => await SalvarValor(nomeChave, valor, minutosParaExpirar),
                        NomeServicoCache, $"{NomeServicoCache} Salvar async", "", param.ToString());

                    metricasCache.Hit(nomeChave, "POST");
                }
            }
            catch (Exception e)
            {
                metricasCache.Fail(nomeChave, "POST");
                var mensagem = new LogMensagem($"Erro ao salvar cache - {NomeServicoCache}",
                    LogNivel.Alerta.ToString(),
                    LogContexto.Cache.ToString(),
                    $"Nome chave: {nomeChave}",
                    "SGP",
                    e.StackTrace,
                    e.InnerException?.Message,
                    e.InnerException?.ToString());
                
                await servicoMensageriaLogs.Publicar(mensagem, RotasRabbitLogs.RotaLogs, ExchangeSgpRabbit.SgpLogs, "PublicarFilaLog");
            }
        }

        public async Task SalvarAsync(string nomeChave, object valor, int minutosParaExpirar = 720, bool utilizarGZip = false)
        {
            await SalvarAsync(nomeChave, JsonConvert.SerializeObject(valor), minutosParaExpirar, utilizarGZip);
        }
    }
}