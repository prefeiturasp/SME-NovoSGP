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
            Comandos.Add(RotasRabbitSgpPendencias.PendenciasGerais, new ComandoRabbit("Pendencias gerais", typeof(IExecutaVerificacaoPendenciasGeraisUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.PendenciasGeraisCalendario, new ComandoRabbit("Pendencias gerais", typeof(IExecutaVerificacaoPendenciasGeraisCalendarioUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.PendenciasGeraisEventos, new ComandoRabbit("Pendencias gerais", typeof(IExecutaVerificacaoPendenciasGeraisEventosUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExecutaVerificacaoPendenciasProfessor, new ComandoRabbit("Executa verificação de pendências de avaliação do professor", typeof(IExecutaVerificacaoGeracaoPendenciaProfessorAvaliacaoUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaTratarAtribuicaoPendenciaUsuarios, new ComandoRabbit("Tratar atribuição de pendência aos usuários", typeof(ITratarAtribuicaoPendenciasUsuariosUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaCargaAtribuicaoPendenciaPerfilUsuario, new ComandoRabbit("Carga de atribuição de pendência aos usuários com perfil", typeof(ICargaAtribuicaoPendenciasPerfilUsuarioUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuarios, new ComandoRabbit("Remover a atribuição de pendência aos usuários", typeof(IRemoverAtribuicaoPendenciasUsuariosUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuariosUe, new ComandoRabbit("Remover a atribuição de pendência aos usuários por UE", typeof(IRemoverAtribuicaoPendenciasUsuariosUeUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuariosUeFuncionario, new ComandoRabbit("Remover a atribuição de pendência aos usuários por UE e por Funcionário", typeof(IRemoverAtribuicaoPendenciasUsuariosUeFuncionarioUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasDevolutiva, new ComandoRabbit("Executar exclusão de pendências de devolutivas", typeof(IExecutarExclusaoPendenciasDevolutivaUseCase)));
        }
    }
}
