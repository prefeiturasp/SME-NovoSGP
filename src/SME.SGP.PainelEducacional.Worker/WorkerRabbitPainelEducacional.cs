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
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional.Frequencia;

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
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarIdebPainelEducacional, new ComandoRabbit("Consolidar Ideb para o Painel Educacional", typeof(IConsolidarIdebPainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarNivelEscritaAlfabetizacao, new ComandoRabbit("Sincronização e Consolidação dos Dados da Sondagem do Nível de Escrita", typeof(IConsolidarInformacoesNivelEscritaAlfabetizacaoPainelEducacionalUseCase), false));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarNivelEscritaAlfabetizacaoCritico, new ComandoRabbit("Sincronização e Consolidação dos Dados da Sondagem da Alfabetização Crítica Escrita", typeof(IConsolidarInformacoesAlfabetizacaoCriticaEscritaPainelEducacionalUseCase), false));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarInformacoesPapPainelEducacional, new ComandoRabbit("Sincronização e Consolidação dos Dados dos IndicadoresDre PAP", typeof(IConsolidarInformacoesPapPainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarVisaoGeralPainelEducacional, new ComandoRabbit("Consolidar Visão Geral para o Painel Educacional", typeof(IConsolidarVisaoGeralPainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarFluenciaLeitoraPainelEducacional, new ComandoRabbit("Consolidar Fluência Leitora para o Painel Educacional", typeof(IConsolidarFluenciaLeitoraPainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarTaxaAlfabetizacaoPainelEducacional, new ComandoRabbit("Consolidar Taxa de Alfabetização para o Painel Educacional", typeof(IConsolidarTaxaAlfabetizacaoPainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarSondagemEscritaUePainelEducacional, new ComandoRabbit("Consolidar Sondagem Escrita Ue para o Painel Educacional", typeof(IConsolidarSondagemEscritaUePainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarAbandonoPainelEducacional, new ComandoRabbit("Consolidar Abandono para o Painel Educacional", typeof(IConsolidarAbandonoPainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarNotasPainelEducacional, new ComandoRabbit("Consolidar Notas para o Painel Educacional", typeof(IConsolidarNotasPainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarReclassificacaoPainelEducacional, new ComandoRabbit("Consolidar Reclassificação para o Painel Educacional", typeof(IConsolidarReclassificacaoPainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarFrequenciaDiariaPainelEducacional, new ComandoRabbit("Consolidar frequência diária para o Painel Educacional", typeof(IConsolidarFrequenciaDiariaPainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarDistorcaoSerieIdadePainelEducacional, new ComandoRabbit("Consolidar Distorção Idade para o Painel Educacional", typeof(IConsolidarDistorcaoIdadePainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarProficienciaIdebPainelEducacional, new ComandoRabbit("Consolidar Proficiência Ideb para o Painel Educacional", typeof(IConsolidarProficienciaIdebPainelEducacionalUseCase)));
            Comandos.Add(RotasRabbitSgpPainelEducacional.ConsolidarProficienciaIdepPainelEducacional, new ComandoRabbit("Consolidar Proficiência Idep para o Painel Educacional", typeof(IConsolidarProficienciaIdepPainelEducacionalUseCase)));
        }
    }
}
