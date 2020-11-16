﻿using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaAlunoDisciplinaPeriodo : IRepositorioBase<FrequenciaAluno>
    {
        FrequenciaAluno Obter(string codigoAluno, string disciplinaId, long periodoEscolarId, TipoFrequenciaAluno tipoFrequencia, string turmaId);

        FrequenciaAluno ObterPorAlunoData(string codigoAluno, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string disciplinaId = "", string codigoTurma = "");
        Task<IEnumerable<FrequenciaAluno>> ObterPorTurma(string turmaCodigo, string disciplinaId, long periodoEscolarId, TipoFrequenciaAluno tipoFrequencia);


        Task<FrequenciaAluno> ObterPorAlunoBimestreAsync(string codigoAluno, int bimestre, TipoFrequenciaAluno tipoFrequencia, string disciplinaId = "");

        FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string disciplinaId, DateTime dataAtual);

        IEnumerable<FrequenciaAluno> ObterAlunosComAusenciaPorDisciplinaNoPeriodo(long periodoId, bool eja);

        IEnumerable<AlunoFaltosoBimestreDto> ObterAlunosFaltososBimestre(ModalidadeTipoCalendario modalidade, double percentualFrequenciaMinimo, int bimestre, int? anoLetivo);

        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaGeralAluno(string alunoCodigo, string turmaCodigo, string componenteCurricularCodigo = "");

        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaBimestresAsync(string codigoAluno, int bimestre, string codigoTurma);
    }
}