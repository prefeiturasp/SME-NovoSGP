using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPlanoAEEVersao : IRepositorioBase<PlanoAEEVersao>
    {
        Task<IEnumerable<PlanoAEEVersaoDto>> ObterVersoesPorPlanoId(long planoId);

        Task<int> ObterMaiorVersaoPlanoPorAlunoCodigo(string codigoAluno);

    }
}
