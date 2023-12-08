using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterUEsPorDreUseCase : AbstractUseCase, IObterUEsPorDreUseCase
    {
        public ObterUEsPorDreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> Executar(string codigoDre, Modalidade? modalidade, int periodo = 0, bool consideraHistorico = false, int anoLetivo = 0, bool consideraNovasUEs = false, bool filtrarTipoEscolaPorAnoLetivo = false, string filtro = "")
        {
            var login = await mediator.Send(ObterLoginAtualQuery.Instance);
            var perfil = await mediator.Send(ObterPerfilAtualQuery.Instance);
            var filtroEhCodigo = !string.IsNullOrWhiteSpace(filtro) && filtro.All(char.IsDigit);

            return await mediator.Send(new ObterUEsPorDREQuery(codigoDre, login, perfil, modalidade, periodo, consideraHistorico, anoLetivo, consideraNovasUEs, filtrarTipoEscolaPorAnoLetivo, filtro, filtroEhCodigo));
        }
    }
}
