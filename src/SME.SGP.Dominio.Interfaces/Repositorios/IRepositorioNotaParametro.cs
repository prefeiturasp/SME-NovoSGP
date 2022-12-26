using SME.SGP.Infra.Dtos.Notas;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioNotaParametro : IRepositorioBase<NotaParametro>
    {
       Task<NotaParametro> ObterPorDataAvaliacao(DateTime dataAvaliacao);
       Task<NotaParametroDto> ObterDtoPorDataAvaliacao(DateTime dataAvaliacao);
    }
}