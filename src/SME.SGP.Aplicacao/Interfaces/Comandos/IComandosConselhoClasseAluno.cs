using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosConselhoClasseAluno
    {
        Task<AuditoriaConselhoClasseAlunoDto> SalvarAsync(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto);
        Task<ParecerConclusivoDto> GerarParecerConclusivoAsync(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo);
    }
}
