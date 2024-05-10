using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasUe
    {
        Task<IEnumerable<TurmaRetornoDto>> ObterTurmas(string ueCodigo, int modalidadeId, int ano, bool ehHistorico);

        Ue ObterPorId(long id);

        Ue ObterPorCodigo(string codigoUe);
    }
}