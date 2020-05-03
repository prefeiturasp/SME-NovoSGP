using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoCalculoParecerConclusivo
    {
        Task<ConselhoClasseParecerConclusivo> Calcular(string alunoCodigo, string turmaCodigo, IEnumerable<ConselhoClasseParecerConclusivo> pareceresDaTurma);
    }
}
