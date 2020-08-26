using MediatR;
using Microsoft.Extensions.Configuration;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReceberRelatorioProntoUseCase : IReceberRelatorioProntoUseCase
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IConfiguration configuration;

        public ReceberRelatorioProntoUseCase(IMediator mediator, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {

            var relatorioCorrelacao = await mediator.Send(new ObterCorrelacaoRelatorioQuery(mensagemRabbit.CodigoCorrelacao));
                if (relatorioCorrelacao == null)
                {
                    throw new NegocioException($"Não foi possível obter a correlação do relatório pronto {mensagemRabbit.CodigoCorrelacao}");
                }

                SentrySdk.AddBreadcrumb($"Correlação obtida com sucesso {relatorioCorrelacao.Codigo}", "9 - ReceberRelatorioProntoUseCase");

                unitOfWork.IniciarTransacao();
                
                if (relatorioCorrelacao.EhRelatorioJasper)
                {
                    var receberRelatorioProntoCommand = mensagemRabbit.ObterObjetoMensagem<ReceberRelatorioProntoCommand>();
                    receberRelatorioProntoCommand.RelatorioCorrelacao = relatorioCorrelacao;

                    var relatorioCorrelacaoJasper = await mediator.Send(receberRelatorioProntoCommand);

                    SentrySdk.AddBreadcrumb("Salvando Correlação Relatório Jasper de retorno", "9 - ReceberRelatorioProntoUseCase");

                    relatorioCorrelacao.AdicionarCorrelacaoJasper(relatorioCorrelacaoJasper);
                }

            var mensagem = mensagemRabbit.ObterObjetoMensagem<MensagemRelatorioProntoDto>();
            switch (relatorioCorrelacao.TipoRelatorio)
            {
                case TipoRelatorio.RelatorioExemplo:
                    break;
                case TipoRelatorio.Boletim:
                case TipoRelatorio.ConselhoClasseAluno:
                case TipoRelatorio.ConselhoClasseTurma:
                case TipoRelatorio.ConselhoClasseAtaFinal:
                case TipoRelatorio.FaltasFrequencia:
                case TipoRelatorio.FechamentoPendencias:
                    SentrySdk.AddBreadcrumb($"Enviando notificação..", $"{relatorioCorrelacao.Codigo.ToString().Substring(0,3)}{relatorioCorrelacao.TipoRelatorio.ShortName()}");
                    await EnviaNotificacaoCriador(relatorioCorrelacao, mensagem.MensagemUsuario);
                    break;
                default:
                    await EnviaNotificacaoCriador(relatorioCorrelacao, mensagem.MensagemUsuario);
                    break;
            }


            unitOfWork.PersistirTransacao();
            SentrySdk.CaptureMessage("9 - ReceberRelatorioProntoUseCase -> Finalizado Fluxo de relatórios");


            return await Task.FromResult(true);
        }

        private async Task EnviaNotificacaoCriador(RelatorioCorrelacao relatorioCorrelacao, string mensagemUsuario)
        {
            var urlRedirecionamentoBase = configuration.GetValue<string>("UrlServidorRelatorios");

            await mediator.Send(new EnviaNotificacaoCriadorCommand(relatorioCorrelacao, urlRedirecionamentoBase, mensagemUsuario));
        }
    }
}
