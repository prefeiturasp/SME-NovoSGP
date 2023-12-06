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
    public class WorkerRabbitPendencias : WorkerRabbitAplicacao
    {
        public WorkerRabbitPendencias(IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitPendencias",
                typeof(RotasRabbitSgpPendencias))
        {
        }

        protected override void RegistrarUseCases()
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
            Comandos.Add(RotasRabbitSgpPendencias.RotaExecutarReplicarParametrosAnoAnterior, new ComandoRabbit("Replicar Parâmetros para ano anterior por modalidade e ano", typeof(IReplicarParametrosAnoAnteriorUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendario, new ComandoRabbit("Buscar UEs para Excluir Pendências Calendário do Ano Anterior", typeof(IExcluirPendenciaCalendarioAnoAnteriorCalendarioUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioUe, new ComandoRabbit("Excluir Pendências Calendário do Ano Anterior Por UE", typeof(IExcluirPendenciaCalendarioAnoAnteriorCalendarioPoUeUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExcluirPendenciaCalendarioAnoAnteriorCalendarioIdsPendencias, new ComandoRabbit("Excluir Pendências Calendário do Ano Anterior Por IDs das Pendencias", typeof(IRemoverPendenciasCalendarioNoFinalDoAnoLetivoUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasDevolutiva, new ComandoRabbit("Executar exclusão de pendências de devolutivas", typeof(IExecutarExclusaoPendenciasDevolutivaUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivoPorAno, new ComandoRabbit("Executar exclusão de pendências no final do ano letivo por ano", typeof(IRemoverPendenciasNoFinalDoAnoLetivoPorAnoUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivoPorUe, new ComandoRabbit("Executar exclusão de pendências no final do ano letivo por ue", typeof(IRemoverPendenciasNoFinalDoAnoLetivoPorUeUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasDiarioDeClasseNoFinalDoAnoLetivo, new ComandoRabbit("Executar exclusão de pendências de diário de classe no final do ano letivo", typeof(IRemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaExecutarExclusaoPendenciasNoFinalDoAnoLetivo, new ComandoRabbit("Executar exclusão de pendências no final do ano letivo", typeof(IRemoverPendenciasNoFinalDoAnoLetivoUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.ExecutarAtualizacaoDosTotalizadoresDasPendencias, new ComandoRabbit("Obter UEs para atualizar a tabela de pendência", typeof(IObterQuantidadeAulaDiaPendenciaUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaBuscarAdicionarQuantidadeAulaDiaPendenciaUe, new ComandoRabbit("Obter Por UE a Quantidade de Aulas e Quantidade de Dias para atualizar a tabela de pendência", typeof(IObterQuantidadeAulaDiaPendenciaPorUeUseCase)));
            Comandos.Add(RotasRabbitSgpPendencias.RotaCargaAdicionarQuantidadeAulaDiaPendencia, new ComandoRabbit("Carga de Quantidade de Aulas e Quantidade de Dias na tabela de pendência", typeof(ICargaQuantidadeAulaDiaPendenciaUseCase)));
        }
    }
}
