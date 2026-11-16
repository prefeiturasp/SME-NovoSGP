using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra.Dtos;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConfiguracaoCacheAbrangenciaQueryHandler : IRequestHandler<ObterConfiguracaoCacheAbrangenciaQuery, ConfiguracaoCacheAbrangenciaDto>
    {
        // Leitura do parâmetro guardada em memória do pod: qualquer mudança no parâmetro vale em até 1 min, sem DEL.
        private static readonly TimeSpan TEMPO_CACHE_PARAMETRO = TimeSpan.FromMinutes(1);

        private readonly IMediator mediator;
        private readonly IMemoryCache memoryCache;

        public ObterConfiguracaoCacheAbrangenciaQueryHandler(IMediator mediator, IMemoryCache memoryCache)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        public async Task<ConfiguracaoCacheAbrangenciaDto> Handle(ObterConfiguracaoCacheAbrangenciaQuery request, CancellationToken cancellationToken)
        {
            // Guarda também o "desligado" (null), para não consultar o banco a cada request.
            if (memoryCache.TryGetValue(NomeChaveCache.PARAMETRO_UTILIZAR_CACHE_ABRANGENCIA, out ConfiguracaoCacheAbrangenciaDto configuracao))
                return configuracao;

            configuracao = await ObterConfiguracaoDoParametro();
            memoryCache.Set(NomeChaveCache.PARAMETRO_UTILIZAR_CACHE_ABRANGENCIA, configuracao, TEMPO_CACHE_PARAMETRO);

            return configuracao;
        }

        private async Task<ConfiguracaoCacheAbrangenciaDto> ObterConfiguracaoDoParametro()
        {
            string valor;

            try
            {
                // select valor ... where tipo = @tipo and ativo → null quando inativo ou inexistente
                valor = await mediator.Send(new ObterParametroSistemaPorTipoQuery(TipoParametroSistema.UtilizarCacheAbrangencia));
            }
            catch (Exception ex)
            {
                // Falha ao ler o parâmetro não pode quebrar os filtros: segue sem cache, como hoje.
                await RegistrarLog($"Erro ao ler o parâmetro UtilizarCacheAbrangencia. Cache de abrangência desligado: {ex.Message}");
                return null;
            }

            if (valor.EhNulo())
                return null;

            ConfiguracaoCacheAbrangenciaDto configuracao = null;

            try
            {
                configuracao = JsonConvert.DeserializeObject<ConfiguracaoCacheAbrangenciaDto>(valor);
            }
            catch
            {
                // JSON inválido: tratado abaixo com os valores padrão
            }

            if (configuracao.EhNulo() || !configuracao.EhValida())
            {
                await RegistrarLog($"Valor inválido no parâmetro UtilizarCacheAbrangencia ('{valor}'). Usando padrão: " +
                                   $"minutos = {ConfiguracaoCacheAbrangenciaDto.MINUTOS_PADRAO}, cj = {ConfiguracaoCacheAbrangenciaDto.MINUTOS_CJ_PADRAO}.");
                return new ConfiguracaoCacheAbrangenciaDto();
            }

            return configuracao;
        }

        private async Task RegistrarLog(string mensagem)
        {
            try
            {
                await mediator.Send(new SalvarLogViaRabbitCommand(mensagem, LogNivel.Alerta, LogContexto.Abrangencia));
            }
            catch
            {
                // Log é melhor esforço: não pode quebrar o filtro
            }
        }
    }
}
