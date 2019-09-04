using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoEOL
    {
        IEnumerable<DreRespostaEolDto> ObterDres();

        IEnumerable<EscolasRetornoDto> ObterEscolasPorCodigo(string[] codigoUes);

        IEnumerable<EscolasRetornoDto> ObterEscolasPorDre(string dreId);

        IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorCodigo(string[] codigoUes);

        IEnumerable<SupervisoresRetornoDto> ObterSupervisoresPorDre(string dreId);
    }
}