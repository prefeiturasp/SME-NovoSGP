using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Institucional.Worker
{
    public class WorkerRabbitInstitucional : WorkerRabbitAplicacao
    {
        public WorkerRabbitInstitucional(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitInstitucional",
                typeof(RotasRabbitSgpInstitucional))
        {
        }

        protected override void RegistrarUseCases()
        {
            Comandos.Add(RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalDreSync, new ComandoRabbit("Estrutura Institucional - Sync de Dre", typeof(IExecutarSincronizacaoInstitucionalDreSyncUseCase), true));
            Comandos.Add(RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalDreTratar, new ComandoRabbit("Estrutura Institucional - Tratar uma Dre", typeof(IExecutarSincronizacaoInstitucionalDreTratarUseCase), true));
            Comandos.Add(RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalUeTratar, new ComandoRabbit("Estrutura Institucional - Tratar uma Ue", typeof(IExecutarSincronizacaoInstitucionalUeTratarUseCase), true));
            Comandos.Add(RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalTipoEscolaSync, new ComandoRabbit("Estrutura Institucional - Sync de Tipos de Escola", typeof(IExecutarSincronizacaoInstitucionalTipoEscolaSyncUseCase), true));
            Comandos.Add(RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalTipoEscolaTratar, new ComandoRabbit("Estrutura Institucional - Tratar um Tipo de Escola", typeof(IExecutarSincronizacaoInstitucionalTipoEscolaTratarUseCase), true));
            Comandos.Add(RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalCicloSync, new ComandoRabbit("Estrutura Institucional - Sync de Ciclo Ensino", typeof(IExecutarSincronizacaoInstitucionalCicloSyncUseCase), true));
            Comandos.Add(RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalCicloTratar, new ComandoRabbit("Estrutura Institucional - Tratar um de Ciclo Ensino", typeof(IExecutarSincronizacaoInstitucionalCicloTratarUseCase), true));
            Comandos.Add(RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalTurmasSync, new ComandoRabbit("Estrutura Institucional - Sincronizar Turmas", typeof(IExecutarSincronizacaoInstitucionalTurmaSyncUseCase), true));
            Comandos.Add(RotasRabbitSgpInstitucional.ConsolidacaoMatriculasTurmasDreCarregar, new ComandoRabbit("Carrega os dados de todas as Dres para consolidação de matrículas", typeof(ICarregarDresConsolidacaoMatriculaUseCase), true));
            Comandos.Add(RotasRabbitSgpInstitucional.SincronizarDresMatriculasTurmas, new ComandoRabbit("Consolidação de matrículas por turmas - Sincronizar Dres", typeof(IExecutarSincronizacaoDresConsolidacaoMatriculasUseCase), true));
            Comandos.Add(RotasRabbitSgpInstitucional.ConsolidacaoMatriculasTurmasCarregar, new ComandoRabbit("Consolidação de matrículas por turmas - Carregar", typeof(ICarregarMatriculaTurmaUseCase), true));
            Comandos.Add(RotasRabbitSgpInstitucional.ConsolidacaoMatriculasTurmasSync, new ComandoRabbit("Consolidação de matrículas por turmas - Sincronizar", typeof(IExecutarSincronizacaoConsolidacaoMatriculasTurmasUseCase), true));
            Comandos.Add(RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalTurmaTratar, new ComandoRabbit("Estrutura Institucional - Tratar uma Turma", typeof(IExecutarSincronizacaoInstitucionalTurmaTratarUseCase), true));
            Comandos.Add(RotasRabbitSgpInstitucional.SincronizaEstruturaInstitucionalTurmaExcluirTurmaExtinta, new ComandoRabbit("Estrutura Institucional - Excluir Turmas Extintas", typeof(IExecutarSincronizacaoInstitucionalTurmaExcluirTurmaExtintaUseCase), true));
        }
    }
}
