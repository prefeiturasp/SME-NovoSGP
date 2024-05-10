using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReceberRelatorioProntoUseCase : IReceberRelatorioProntoUseCase
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public ReceberRelatorioProntoUseCase(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var relatorioCorrelacao = await mediator.Send(new ObterCorrelacaoRelatorioQuery(mensagemRabbit.CodigoCorrelacao));
            if (relatorioCorrelacao.EhNulo())
            {
                throw new NegocioException($"Não foi possível obter a correlação do relatório pronto {mensagemRabbit.CodigoCorrelacao}");
            }

            if (relatorioCorrelacao.EhRelatorioJasper)
            {
                var receberRelatorioProntoCommand = mensagemRabbit.ObterObjetoMensagem<ReceberRelatorioProntoCommand>();
                receberRelatorioProntoCommand.RelatorioCorrelacao = relatorioCorrelacao;

                var relatorioCorrelacaoJasper = await mediator.Send(receberRelatorioProntoCommand);

                relatorioCorrelacao.AdicionarCorrelacaoJasper(relatorioCorrelacaoJasper);
            }

            var mensagem = mensagemRabbit.ObterObjetoMensagem<MensagemRelatorioProntoDto>();
            var urlRedirecionamentoBase = configuration.GetSection("UrlServidorRelatorios").Value;
            switch (relatorioCorrelacao.TipoRelatorio)
            {
                case TipoRelatorio.RelatorioExemplo:
                    break;
                case TipoRelatorio.Boletim:
                case TipoRelatorio.ConselhoClasseAluno:
                case TipoRelatorio.ConselhoClasseTurma:
                case TipoRelatorio.ConselhoClasseAtaFinal:
                case TipoRelatorio.Frequencia:
                case TipoRelatorio.Pendencias:
                    await EnviaNotificacaoCriador(relatorioCorrelacao, mensagem.MensagemUsuario, mensagem.MensagemTitulo, urlRedirecionamentoBase);
                    break;
                default:
                    await EnviaNotificacaoCriador(relatorioCorrelacao, mensagem.MensagemUsuario, mensagem.MensagemTitulo, urlRedirecionamentoBase);
                    break;
            }

            return await Task.FromResult(true);
        }
        private async Task EnviaNotificacaoCriador(RelatorioCorrelacao relatorioCorrelacao, string mensagemUsuario, string mensagemTitulo, string urlRedirecionamentoBase)
        {
            await mediator.Send(new EnviaNotificacaoCriadorCommand(relatorioCorrelacao, urlRedirecionamentoBase, mensagemUsuario, mensagemTitulo));
        }
    }
}
