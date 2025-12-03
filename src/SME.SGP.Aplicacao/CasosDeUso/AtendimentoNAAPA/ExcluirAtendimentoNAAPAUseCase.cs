using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAtendimentoNAAPAUseCase : AbstractUseCase, IExcluirAtendimentoNAAPAUseCase
    {
        public ExcluirAtendimentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long encaminhamentoNAAPAId)
        {
            var encaminhamentoNAAPA = await mediator.Send(new ObterEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPAId));

            if (encaminhamentoNAAPA.EhNulo())
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_ENCONTRADO);
            
            if (encaminhamentoNAAPA.Situacao != SituacaoNAAPA.Rascunho && encaminhamentoNAAPA.Situacao != SituacaoNAAPA.AguardandoAtendimento)
                throw new NegocioException(MensagemNegocioEncaminhamentoNAAPA.ENCAMINHAMENTO_NAO_PODE_SER_EXCLUIDO_NESSA_SITUACAO);
            
            return (await mediator.Send(new ExcluirEncaminhamentoNAAPACommand(encaminhamentoNAAPAId)));
        }

       
    }
}
