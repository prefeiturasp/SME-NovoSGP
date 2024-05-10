using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class CadastrarParecerCPPlanoAEEUseCase : AbstractUseCase, ICadastrarParecerCPPlanoAEEUseCase
    {
        public CadastrarParecerCPPlanoAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(long planoAEEId, PlanoAEECadastroParecerDto planoDto)
            => await mediator.Send(new CadastrarParecerCPCommand(planoAEEId, planoDto.Parecer));
    }
}