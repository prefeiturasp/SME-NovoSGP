using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosConselhoClasseAluno
    {
        Task<ConselhoClasseAluno> SalvarAsync(ConselhoClasseAlunoAnotacoesDto conselhoClasseAlunoDto);
        Task<ParecerConclusivoDto> GerarParecerConclusivoAsync(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo, bool consideraHistorico);
    }
}
