using SME.SGP.Aplicacao.Integracoes.Respostas;
using System.Collections.Generic;
using System.Threading.Tasks;

using SME.SGP.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoEOL
    {
        Task<IEnumerable<TurmaDto>> ObterTurmasAtribuidasAoProfessorPorEscolaEAnoLetivo(string rfProfessor, long codigoEscola, int anoLetivo);
        Task<IEnumerable<DisciplinaResposta>> ObterDisciplinasPorProfessorETurma(long codigoTurma, string rfProfessor);
    }
}