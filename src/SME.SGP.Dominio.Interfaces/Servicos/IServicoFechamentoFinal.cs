using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFechamentoFinal
    {
        Task<AuditoriaPersistenciaDto> SalvarAsync(FechamentoTurmaDisciplina fechamentoFinal, Turma turma, Usuario usuarioLogado, IList<Infra.FechamentoFinalSalvarItemDto> notasDto, bool emAprovacao);

        Task VerificaPersistenciaGeral(Turma turma);
    }
}