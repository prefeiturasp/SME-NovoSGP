using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.Informes
{
    public class ExcluirInformesUseCase : AbstractUseCase, IExcluirInformesUseCase
    {
        public ExcluirInformesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long param)
        {
            var existeInformes = await mediator.Send(new ExisteInformePorIdQuery(param));

            if (!existeInformes)
                throw new NegocioException(MensagemNegocioInformes.INFORMES_NAO_ENCONTRADO);

            return await mediator.Send(new ExcluirInformesCommand(param)); 
        }
    }
}
