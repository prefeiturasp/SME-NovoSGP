using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SolicitarExclusaoComunicadosEscolaAquiUseCase : AbstractUseCase, ISolicitarExclusaoComunicadosEscolaAquiUseCase
    {
        public SolicitarExclusaoComunicadosEscolaAquiUseCase(IMediator mediator) : base(mediator)
        {
        }
        public async Task<bool> Executar(long[] ids)
            => await mediator.Send(new ExcluirComunicadoCommand(ids));       
    }
}
