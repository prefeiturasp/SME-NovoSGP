using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Aplicacao
{
    public class ExcluirRegistroAcaoUseCase : AbstractUseCase, IExcluirRegistroAcaoUseCase
    {
        public ExcluirRegistroAcaoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long registroAcaoid)
        {
            var registroAcao = await mediator.Send(new ObterRegistroAcaoPorIdQuery(registroAcaoid));

            if (registroAcao.EhNulo())
                throw new NegocioException(MensagemNegocioRegistroAcao.REGISTROACAO_NAO_ENCONTRADO);
            
            return (await mediator.Send(new ExcluirRegistroAcaoCommand(registroAcaoid)));
        }

       
    }
}
