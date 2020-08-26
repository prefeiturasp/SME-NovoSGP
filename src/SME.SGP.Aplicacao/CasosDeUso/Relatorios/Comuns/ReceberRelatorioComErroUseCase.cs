using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReceberRelatorioComErroUseCase : IReceberRelatorioComErroUseCase
    {
        private readonly IMediator mediator;

        public ReceberRelatorioComErroUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var erro = mensagemRabbit.ObterObjetoMensagem<RetornoWorkerDto>();
            var relatorioCorrelacao = await mediator.Send(new ObterCorrelacaoRelatorioQuery(mensagemRabbit.CodigoCorrelacao));

            if (relatorioCorrelacao == null)
            {
                throw new NegocioException($"Não foi possível obter a correlação do relatório pronto {mensagemRabbit.CodigoCorrelacao}");
            }

            var command = new NotificarUsuarioCommand($"{relatorioCorrelacao.TipoRelatorio.Description()} - Erro ao gerar relatório.",
                                                      $"Ocorreu um erro na geração do seu '{relatorioCorrelacao.TipoRelatorio.Description()}'.{System.Environment.NewLine}{erro.Mensagem}",
                                                      mensagemRabbit.UsuarioLogadoRF,
                                                      NotificacaoCategoria.Aviso,
                                                      NotificacaoTipo.Relatorio);

            await mediator.Send(command);
            return await Task.FromResult(true);
        }
    }
}
