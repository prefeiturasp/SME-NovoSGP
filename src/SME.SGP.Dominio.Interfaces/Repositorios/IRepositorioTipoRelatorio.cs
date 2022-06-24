using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;
namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTipoRelatorio
    {
        Task<int>ObterTipoPorCodigo(string codigo);        
    }

}
