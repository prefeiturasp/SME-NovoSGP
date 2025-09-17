using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;
using System.Diagnostics.CodeAnalysis;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Interfaces;

namespace SME.SGP.PainelEducacional.Worker
{
    [ExcludeFromCodeCoverage]
    public class WorkerRabbitPainelEducacional : WorkerRabbitAplicacao
    {
        public WorkerRabbitPainelEducacional(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitPainelEducacional",
                typeof(RotasRabbitSgpPainelEducacional))
        {

        }


        protected override void RegistrarUseCases()
        {
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarIdepPainelEducacional, new ComandoRabbit("Consolidar idep para painel educacional", typeof(IConsolidarIdepPainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarNivelEscritaAlfabetizacao, new ComandoRabbit("Sincronização e Consolidação dos Dados da Sondagem do Nível de Escrita", typeof(IConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCase), false));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarNivelEscritaAlfabetizacaoCritico, new ComandoRabbit("Sincronização e Consolidação dos Dados da Sondagem da Alfabetização Crítica Escrita", typeof(IConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCase), false));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarInformacoesPapPainelEducacional, new ComandoRabbit("Sincronização e Consolidação dos Dados de PAP", typeof(IConsolidarInformacoesPapPainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarVisaoGeralPainelEducacional, new ComandoRabbit("Consolidar Visão Geral para o Painel Educacional", typeof(IConsolidarVisaoGeralPainelEducacionalUseCase)));
        }
    }
}
