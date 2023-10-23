using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterGruposDeUsuariosUseCase : AbstractUseCase, IObterGruposDeUsuariosUseCase
    {
        public ObterGruposDeUsuariosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public Task<IEnumerable<GruposDeUsuariosDto>> Executar(int tipoPerfil)
        {
            return mediator.Send(new ObterGruposDeUsuariosQuery(tipoPerfil));
        }
    }
}
