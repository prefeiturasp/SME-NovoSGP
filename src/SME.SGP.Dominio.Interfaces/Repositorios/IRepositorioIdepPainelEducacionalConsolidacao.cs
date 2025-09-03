using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioIdepPainelEducacionalConsolidacao
    {
        Task<bool> Inserir(IEnumerable<PainelEducacionalIdep> consolidacoes);
    }
}
