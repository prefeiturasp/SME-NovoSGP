using SME.SGP.Infra;
using System.Collections;
using System.Collections.Generic;
﻿using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFrequencia
    {
        Task<FrequenciaDto> ObterListaFrequenciaPorAula(long aulaId);

        Task<bool> FrequenciaAulaRegistrada(long aulaId);
      
        Task<IEnumerable<AlunoAusenteDto>> ObterListaAlunosComAusencia(string turmaId, string disciplinaId, int bimestre);
      
        FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string disciplinaId, DateTime dataAtual);

        SinteseDto ObterSinteseAluno(string codigoAluno, DateTime dataReferencia, string disciplinaId);

    }
}