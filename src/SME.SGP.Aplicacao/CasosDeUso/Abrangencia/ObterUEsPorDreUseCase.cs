using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUEsPorDreUseCase : AbstractUseCase, IObterUEsPorDreUseCase
    {
        public ObterUEsPorDreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> Executar(string codigoDre, Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, bool consideraNovasUEs = false)
        {
            var login = await mediator.Send(new ObterLoginAtualQuery());
            var perfil = await mediator.Send(new ObterPerfilAtualQuery());

            return await mediator.Send(new ObterUEsPorDREQuery(codigoDre, login, perfil, modalidade, periodo, consideraHistorico, anoLetivo, consideraNovasUEs));
        }
    }
}
