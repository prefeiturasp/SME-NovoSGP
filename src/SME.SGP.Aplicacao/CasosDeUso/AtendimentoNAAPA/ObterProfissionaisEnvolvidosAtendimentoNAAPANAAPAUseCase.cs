using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase : IObterProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterProfissionaisEnvolvidosAtendimentoNAAPANAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<FuncionarioUnidadeDto>> Executar(FiltroBuscarProfissionaisEnvolvidosAtendimentoNAAPA filtro)
        => await mediator.Send(new ObterResponsaveisPorDreUeNAAPAQuery(filtro.CodigoDre, filtro.CodigoUe));
    }
}
