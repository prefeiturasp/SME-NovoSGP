using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IConsultaDres
    {
        IEnumerable<UnidadeEscolarDto> ObterEscolasSemAtribuicao(string dreId);

        IEnumerable<DreConsultaDto> ObterTodos();
    }
}