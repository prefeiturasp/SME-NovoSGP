using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasProfessor
    {
        IEnumerable<ProfessorTurmaDto> Listar(string codigoRf);

        Task<IEnumerable<ProfessorResumoDto>> ObterResumoAutoComplete(int anoLetivo, string dreId, string nomeProfessor);

        Task<ProfessorResumoDto> ObterResumoPorRFAnoLetivo(string codigoRF, int anoLetivo);

        Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo);
    }
}