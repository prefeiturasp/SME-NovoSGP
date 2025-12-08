using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.NAAPA.Worker
{
    public class WorkerRabbitNAAPA : WorkerRabbitAplicacao
    {
        public WorkerRabbitNAAPA(
            IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IServicoMensageriaMetricas servicoMensageriaMetricas,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, servicoMensageriaMetricas,
                telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitNAAPA", typeof(RotasRabbitSgpNAAPA))
        {
        }

        protected override void RegistrarUseCases()
        {
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarAtualizacaoDasInformacoesEncaminhamentoNAAPA, new ComandoRabbit("Atualiza informações do encaminhamento NAAPA", typeof(IAtualizarInformacoesDoAtendimentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarAtualizacaoDaTurmaDoEncaminhamentoNAAPA, new ComandoRabbit("Atualiza turma do encaminhamento NAAPA", typeof(IAtualizarTurmaDoAtendimentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarAtualizacaoDoEnderecoDoEncaminhamentoNAAPA, new ComandoRabbit("Atualiza Endereço do encaminhamento NAAPA", typeof(IAtualizarEnderecoDoAtendimentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarNotificacaoAtualizacaoSituacaoAlunoDoEncaminhamentoNAAPA, new ComandoRabbit("Notifica responsáveis sobre inativação de aluno na turma do encaminhamento NAAPA", typeof(INotificarSobreInativacaoAlunoTurmaDoAtendimentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarNotificacaoTransferenciaUeDreDoEncaminhamentoNAAPA, new ComandoRabbit("Notifica responsáveis sobre transferencia do aluno entre ues e dres do encaminhamento NAAPA", typeof(INotificarSobreTransferenciaUeDreAlunoTurmaDoAtendimentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarAtualizacaoDasTurmasProgramaDoEncaminhamentoNAAPA, new ComandoRabbit("Atualiza Turmas de Programa do encaminhamento NAAPA", typeof(IAtualizarTurmasProgramaDoAtendimentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarCargaConsolidadoEncaminhamentoNAAPA,new ComandoRabbit("Executar Carga Consolidado Encaminhamento NAAPA",typeof(IExecutarCargaConsolidadoAtendimentoNAAPAUseCase),true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarBuscarUesConsolidadoEncaminhamentoNAAPA,new ComandoRabbit("Executar Carga Consolidado Encaminhamento NAAPA Por UE",typeof(IExecutarBuscarUesConsolidadoAtendimentoNAAPAUseCase),true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarInserirConsolidadoEncaminhamentoNAAPA,new ComandoRabbit("Executar Inserir Consolidado Encaminhamento NAAPA",typeof(IExecutarInserirConsolidadoAtendimentoNAAPAUseCase),true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarExcluirConsolidadoEncaminhamentoNAAPA, new ComandoRabbit("Executar Excluir Consolidado Encaminhamento NAAPA", typeof(IExecutarExcluirConsolidadoAtendimentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarBuscarAtendimentosProfissionalConsolidadoEncaminhamentoNAAPA, new ComandoRabbit("Executar Carga Consolidado Encaminhamento NAAPA Por Profissionais", typeof(IExecutarBuscarConsolidadoAtendimentosProfissionalAtendimentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarInserirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPA, new ComandoRabbit("Executar Inserir Consolidado Encaminhamento NAAPA Profissional", typeof(IExecutarInserirConsolidadoAtendimentoProfissionalAtendimentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarExcluirConsolidadoAtendimentoProfissionalEncaminhamentoNAAPA, new ComandoRabbit("Executar Excluir Consolidado Encaminhamento NAAPA Profissional", typeof(IExecutarExcluirConsolidadoAtendimentoProfissionalAtendimentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarNotificacaoInatividadeAtendimentoNAAPA, new ComandoRabbit("Executar notificação de inatividade do atendimento NAAPA", typeof(INotificarInatividadeDoAtendimentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarNotificacaoInatividadeAtendimentoPorUeNAAPA, new ComandoRabbit("Executar notificação de inatividade do atendimento NAAPA por ue", typeof(INotificarInatividadeDoAtendimentoNAAPAPorUeUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarNotificacaoInatividadeAtendimentoInformacaoNAAPA, new ComandoRabbit("Executar notificação de inatividade do atendimento informação NAAPA", typeof(INotificarInatividadeDoAtendimentoNAAPAInformacaoUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.RotaExcluirNotificacaoInatividadeAtendimento, new ComandoRabbit("Executar exclusão notificações de inatividade de atendimentos NAAPA", typeof(IExecutarExclusaoNotificacaoInatividadeAtendimentoNAAPAUseCase), true));
        }
    }
}
