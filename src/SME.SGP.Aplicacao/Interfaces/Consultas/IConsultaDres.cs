using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaDres
    {
        IEnumerable<UnidadeEscolarDto> ObterEscolasPorDre(string dreId);

        IEnumerable<UnidadeEscolarDto> ObterEscolasSemAtribuicao(string dreId);

        IEnumerable<DreConsultaDto> ObterTodos();
    }
}