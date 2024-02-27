using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.ConselhoClasse
{
    public class AlterarParecerConclusivoUseCase : AbstractUseCase, IAlterarParecerConclusivoUseCase
    {
        public AlterarParecerConclusivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ParecerConclusivoDto> Executar(AlterarParecerConclusivoDto param)
        {
            if (param.ParecerConclusivoId.EhNulo())
                throw new NegocioException(MensagemNegocioConselhoClasse.PARECER_CONCLUSIVO_DEVE_SER_INFORMADO);

            return await mediator.Send(new AlterarParecerConclusivoCommand(param));
        }
    }
}
