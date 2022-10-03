using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioPendenciaDiarioBordo : IRepositorioBase<PendenciaDiarioBordo>
    {
        Task Excluir(long aulaId, long componenteCurricularId);
        Task ExcluirPorAulaId(long aulaId);
        Task<bool> VerificarSeExistePendenciaDiarioComPendenciaId(long pendenciaId);
        Task<IEnumerable<AulaComComponenteDto>> ListarPendenciasDiario(string turmaId, long[] componentesCurricularesId);
    }
}
