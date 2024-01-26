using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterGruposDeUsuariosUseCase : AbstractUseCase, IObterGruposDeUsuariosUseCase
    {
        public ObterGruposDeUsuariosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<GruposDeUsuariosDto>> Executar(int tipoPerfil)
        {
            var grupos = await mediator.Send(new ObterGruposDeUsuariosQuery(tipoPerfil));
            var gruposAgrupados = grupos.GroupBy(c => c.GuidPerfil).ToList();

            return gruposAgrupados.Select(g => g.FirstOrDefault());
        }
    }
}
