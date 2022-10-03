﻿using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusenciaAlunoConsulta : IRepositorioBase<CompensacaoAusenciaAluno>
    {
        Task<IEnumerable<CompensacaoAusenciaAluno>> ObterPorCompensacao(long compensacaoId);        
        Task<int> ObterTotalCompensacoesPorAlunoETurmaAsync(int bimestre, string codigoAluno, string disciplinaId, string turmaId);
        Task<IEnumerable<CompensacaoAusenciaAluno>> ObterCompensacoesAluno(string codigoAluno, long compensacaoIgnorada, int bimestre);
        Task<IEnumerable<CompensacaoAusenciaAlunoCalculoFrequenciaDto>> ObterTotalCompensacoesPorAlunosETurmaAsync(int bimestre, List<string> alunos, string turmaCodigo);
    }
}
