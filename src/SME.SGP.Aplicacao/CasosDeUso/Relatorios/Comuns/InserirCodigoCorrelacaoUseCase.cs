using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class InserirCodigoCorrelacaoUseCase : IInserirCodigoCorrelacaoUseCase
    {
        private readonly IMediator mediator;

        public InserirCodigoCorrelacaoUseCase(IMediator mediator, IRepositorioCorrelacaoRelatorio repositorioCorrelacaoRelatorio)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var mensagem = mensagemRabbit.ObterObjetoMensagem<MensagemInserirCodigoCorrelacaoDto>();

            await mediator.Send(new InserirCodigoCorrelacaoCommand(mensagemRabbit.CodigoCorrelacao, mensagemRabbit.UsuarioLogadoRF, mensagem.TipoRelatorio, mensagem.Formato));

            return await Task.FromResult(true);
        }


    }
}
