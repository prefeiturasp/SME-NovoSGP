using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class CadastrarParecerPAAIPlanoAEEUseCase : AbstractUseCase, ICadastrarParecerPAAIPlanoAEEUseCase
    {
        public CadastrarParecerPAAIPlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long planoAEEId, PlanoAEECadastroParecerDto planoDto)
            => await mediator.Send(new CadastrarParecerPAAICommand(planoAEEId, planoDto.Parecer));
    }
}