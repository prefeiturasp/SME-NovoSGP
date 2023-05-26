using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Workers;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Utilitarios;

namespace SME.SGP.NAAPA.Worker
{
    public class WorkerRabbitNAAPA : WorkerRabbitMQBase
    {
        public WorkerRabbitNAAPA(
            IServiceScopeFactory serviceScopeFactory,
            IServicoTelemetria servicoTelemetria,
            IServicoMensageriaSGP servicoMensageria,
            IOptions<TelemetriaOptions> telemetriaOptions,
            IOptions<ConsumoFilasOptions> consumoFilasOptions,
            IConnectionFactory factory) : base(serviceScopeFactory, servicoTelemetria, servicoMensageria, telemetriaOptions, consumoFilasOptions, factory, "WorkerRabbitNAAPA", typeof(RotasRabbitSgpNAAPA))
        {
        }

        protected override void RegistrarUseCasesDoWorker()
        {
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarAtualizacaoDasInformacoesEncaminhamentoNAAPA, new ComandoRabbit("Atualiza informações do encaminhamento NAAPA", typeof(IAtualizarInformacoesDoEncaminhamentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarAtualizacaoDaTurmaDoEncaminhamentoNAAPA, new ComandoRabbit("Atualiza turma do encaminhamento NAAPA", typeof(IAtualizarTurmaDoEncaminhamentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarAtualizacaoDoEnderecoDoEncaminhamentoNAAPA, new ComandoRabbit("Atualiza Endereço do encaminhamento NAAPA", typeof(IAtualizarEnderecoDoEncaminhamentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarNotificacaoAtualizacaoSituacaoAlunoDoEncaminhamentoNAAPA, new ComandoRabbit("Notifica responsáveis sobre inativação de aluno na turma do encaminhamento NAAPA", typeof(INotificarSobreInativacaoAlunoTurmaDoEncaminhamentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarNotificacaoTransferenciaUeDreDoEncaminhamentoNAAPA, new ComandoRabbit("Notifica responsáveis sobre transferencia do aluno entre ues e dres do encaminhamento NAAPA", typeof(INotificarSobreTransferenciaUeDreAlunoTurmaDoEncaminhamentoNAAPAUseCase), true));
            Comandos.Add(RotasRabbitSgpNAAPA.ExecutarAtualizacaoDasTurmasProgramaDoEncaminhamentoNAAPA, new ComandoRabbit("Atualiza Turmas de Programa do encaminhamento NAAPA", typeof(IAtualizarTurmasProgramaDoEncaminhamentoNAAPAUseCase), true));
        }
    }
}
