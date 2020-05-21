using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasConselhoClasse
    {
        ConselhoClasse ObterPorId(long conselhoClasseId);
        Task<(int, bool)> ValidaConselhoClasseUltimoBimestre(Turma turma);
        Task<ConselhoClasseAlunoResumoDto> ObterConselhoClasseTurma(string turmaCodigo, string alunoCodigo, int bimestre = 0, bool ehFinal = false, bool consideraHistorico = false);
    }
}
