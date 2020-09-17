﻿using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFrequencia
    {
        Task<FrequenciaDto> ObterListaFrequenciaPorAula(long aulaId);

        Task<bool> FrequenciaAulaRegistrada(long aulaId);

        Task<IEnumerable<AlunoAusenteDto>> ObterListaAlunosComAusencia(string turmaId, string disciplinaId, int bimestre);

        FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string disciplinaId, DateTime dataAtual);

        Task<SinteseDto> ObterSinteseAluno(double percentualFrequencia, DisciplinaDto disciplina);

        Task<double> ObterFrequenciaMedia(DisciplinaDto disciplina);
        Task<double> ObterFrequenciaGeralAluno(string alunoCodigo, string turmaCodigo, string componenteCurricularCodigo = "");
    }
}