using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ExcluirEncaminhamentoNAAPAUseCase : AbstractUseCase, IExcluirEncaminhamentoNAAPAUseCase
    {
        public ExcluirEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long encaminhamentoNAAPAId)
        {
            var encaminhamentoNAAPA = await mediator.Send(new ObterEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPAId));

            if (encaminhamentoNAAPA == null )
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_ENCONTRADO);
            
            if (encaminhamentoNAAPA.Situacao != SituacaoNAAPA.Rascunho)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_PODE_SER_EXCLUIDO_NESSA_SITUACAO);
            
            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoQuery());

            return (await mediator.Send(new ExcluirEncaminhamentoNAAPACommand(encaminhamentoNAAPAId)));
        }

       
    }
}
