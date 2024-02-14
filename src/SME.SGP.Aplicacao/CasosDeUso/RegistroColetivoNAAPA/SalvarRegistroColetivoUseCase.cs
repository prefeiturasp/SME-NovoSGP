using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarRegistroColetivoUseCase : AbstractUseCase, ISalvarRegistroColetivoUseCase
    {
        public SalvarRegistroColetivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ResultadoRegistroColetivoDto> Executar(RegistroColetivoDto registroColetivo)
        {
            if (registroColetivo.Id.EhNulo())
                return await mediator.Send(new InserirRegistroColetivoCommand(registroColetivo));

            return await mediator.Send(new AlterarRegistroColetivoCommand(registroColetivo));
        }
    }
}
