using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasModalidadesNotasVisaoUeUseCase
    {
        Task<IEnumerable<IdentificacaoInfo>> ObterModalidadesNotasVisaoUe(int anoLetivo, string codigoUe, int bimestre);
    }
}
