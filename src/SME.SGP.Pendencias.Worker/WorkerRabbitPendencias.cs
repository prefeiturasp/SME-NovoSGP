using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.Pendencias.Worker
{
    public class WorkerRabbitPendencias : WorkerRabbitMQBase
    {
        public WorkerRabbitPendencias(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitPendencias",
                typeof(RotasRabbitSgpPendencias))
        {
        }

        protected override void RegistrarUseCasesDoWorker()
        {
            Comandos.Add(RotasRabbitSgpPendencias.PendenciasGerais, new ComandoRabbit("Pendencias gerais", typeof(IExecutaVerificacaoPendenciasGeraisUseCase), true));
            Comandos.Add(RotasRabbitSgpPendencias.PendenciasGeraisCalendario, new ComandoRabbit("Pendencias gerais", typeof(IExecutaVerificacaoPendenciasGeraisCalendarioUseCase), true));
            Comandos.Add(RotasRabbitSgpPendencias.PendenciasGeraisEventos, new ComandoRabbit("Pendencias gerais", typeof(IExecutaVerificacaoPendenciasGeraisEventosUseCase), true));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExecutaVerificacaoPendenciasProfessor, new ComandoRabbit("Executa verificação de pendências de avaliação do professor", typeof(IExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase), true));
            Comandos.Add(RotasRabbitSgpPendencias.RotaTratarAtribuicaoPendenciaUsuarios, new ComandoRabbit("Tratar atribuição de pendência aos usuários", typeof(ITratarAtribuicaoPendenciasUsuariosUseCase), true));
            Comandos.Add(RotasRabbitSgpPendencias.RotaCargaAtribuicaoPendenciaPerfilUsuario, new ComandoRabbit("Carga de atribuição de pendência aos usuários com perfil", typeof(ICargaAtribuicaoPendenciasPerfilUsuarioUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuarios, new ComandoRabbit("Remover a atribuição de pendência aos usuários", typeof(IRemoverAtribuicaoPendenciasUsuariosUseCase), true));
            Comandos.Add(RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuariosUe, new ComandoRabbit("Remover a atribuição de pendência aos usuários por UE", typeof(IRemoverAtribuicaoPendenciasUsuariosUeUseCase), true));
            Comandos.Add(RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuariosUeFuncionario, new ComandoRabbit("Remover a atribuição de pendência aos usuários por UE e por Funcionário", typeof(IRemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCase), true));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasDevolutiva, new ComandoRabbit("Executar exclusão de pendências de devolutivas", typeof(IExecutarExclusaoPendenciasDevolutivaUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivoPorAno, new ComandoRabbit("Executar exclusão de pendências no final do ano letivo por ano", typeof(IRemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivoPorUe, new ComandoRabbit("Executar exclusão de pendências no final do ano letivo por ue", typeof(IRemoverPendenciasNoFinalDoAnoLetivoPorUeUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasDiarioDeClasseNoFinalDoAnoLetivo, new ComandoRabbit("Executar exclusão de pendências de diário de classe no final do ano letivo", typeof(IRemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase)));
        }
    }
}
