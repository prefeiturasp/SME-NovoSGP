using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConceitoConsulta : IRepositorioBase<Conceito>
    {
       Task<IEnumerable<Conceito>> ObterPorData(DateTime dataAvaliacao);
    }
}