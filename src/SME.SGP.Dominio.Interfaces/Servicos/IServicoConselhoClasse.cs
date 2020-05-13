﻿using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoConselhoClasse
    {
        Task<ConselhoClasseNotaRetornoDto> SalvarConselhoClasseAlunoNotaAsync(ConselhoClasseNotaDto conselhoClasseNotaDto, string alunoCodigo, long conselhoClasseId, long fechamentoTurmaId);

        Task<AuditoriaConselhoClasseAlunoDto> SalvarConselhoClasseAluno(ConselhoClasseAluno conselhoClasseAluno);
        Task<ParecerConclusivoDto> GerarParecerConclusivoAlunoAsync(long conselhoClasseId, long fechamentoTurmaId, string alunoCodigo);
        Task<bool> VerificaNotasTodosComponentesCurriculares(string alunoCodigo, Turma turma, long? periodoEscolarId);
    }
}