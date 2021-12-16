using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFechamentoAlunoConsulta : IRepositorioBase<FechamentoAluno>
    {
        Task<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>> ObterAnotacoesTurmaAlunoBimestreAsync(string alunoCodigo, string[] turmasCodigos, long periodoId);

        Task<FechamentoAluno> ObterFechamentoAluno(long fechamentoTurmaDisciplinaId, string codigoAluno);

        Task<FechamentoAluno> ObterFechamentoAlunoENotas(long fechamentoTurmaDisciplinaId, string alunoCodigo);

        Task<IEnumerable<FechamentoAluno>> ObterPorFechamentoTurmaDisciplina(long fechamentoDisciplinaId);
        Task<IEnumerable<TurmaAlunoBimestreFechamentoDto>> ObterAlunosComFechamento(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);
    }
}