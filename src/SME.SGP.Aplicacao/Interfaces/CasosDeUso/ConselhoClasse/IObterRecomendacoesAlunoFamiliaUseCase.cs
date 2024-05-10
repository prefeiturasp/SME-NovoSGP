using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterRecomendacoesAlunoFamiliaUseCase 
    {
        Task<IEnumerable<RecomendacoesAlunoFamiliaDto>> Executar();
    }
}
