using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasUnidadesEscolares
    {
        Task<IEnumerable<UsuarioEolRetornoDto>> ObtemFuncionariosPorUe(BuscaFuncionariosFiltroDto buscaFuncionariosFiltroDto);
    }
}