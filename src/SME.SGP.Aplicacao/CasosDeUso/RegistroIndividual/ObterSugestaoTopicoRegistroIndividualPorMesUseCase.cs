using MediatR;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSugestaoTopicoRegistroIndividualPorMesUseCase : AbstractUseCase, IObterSugestaoTopicoRegistroIndividualPorMesUseCase
    {
        public ObterSugestaoTopicoRegistroIndividualPorMesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<SugestaoTopicoRegistroIndividualDto> Executar(int mes)
        {
            var susgestaoTopico = await mediator.Send(new ObterSugestaoTopicoRegistroIndividualPorMesQuery(mes));

            return susgestaoTopico;
        }
    }
}
