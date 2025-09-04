using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioPainelEducacionalIndicadoresNivelAlfabetizacaoCritica : IRepositorioBase<PainelEducacionalIndicadoresNivelAlfabetizacaoCritica>
    {
        Task<IEnumerable<PainelEducacionalIndicadoresNivelAlfabetizacaoCritica>> ObterNumeroEstudantes(string codigoDre, string codigoUe);
    }
}
