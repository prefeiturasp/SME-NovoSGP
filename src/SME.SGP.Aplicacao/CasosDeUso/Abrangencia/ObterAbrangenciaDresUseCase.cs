using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAbrangenciaDresUseCase : AbstractUseCase, IObterAbrangenciaDresUseCase
    {
        public ObterAbrangenciaDresUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AbrangenciaDreRetornoDto>> Executar(Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, string filtro = "")
        {
            var login = await mediator
                .Send(ObterLoginAtualQuery.Instance);

            var perfil = await mediator
                .Send(ObterPerfilAtualQuery.Instance);

            return await mediator
                .Send(new ObterAbrangenciaDresQuery(login, perfil, modalidade, periodo, consideraHistorico, anoLetivo, filtro));
        }
    }
}