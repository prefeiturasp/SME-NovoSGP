using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioSondagemEscrita
    {
        Task<IEnumerable<SondagemEscritaDto>> ObterSondagemEscritaAsync(string codigoDre, string codigoUe, int anoLetivo, int bimestre, int serieAno);
    }
}
