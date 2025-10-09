using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasSondagemEscritaUseCase
    {
        Task<IEnumerable<SondagemEscritaDto>> ObterSondagemEscrita(string codigoDre, string codigoUe, int anoLetivo, int bimestre, int serieAno);
    }
}
