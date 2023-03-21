using SME.SGP.Infra;
using System.Collections.Generic;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public interface IListarAtribuicoesResponsaveisPorFiltroUseCase : IUseCase<AtribuicaoResponsaveisFiltroDto, IEnumerable<AtribuicaoResponsavelDto>>
    {
    }
}
