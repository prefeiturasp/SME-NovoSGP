using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoEOL
    {
        Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorProfessorETurma(long codigoTurma, string rfProfessor);

        Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, string codigoEscola, int anoLetivo);
        IEnumerable<ProfessorTurmaReposta> ObterListaTurmasPorProfessor(string codigoRf);
    }
}