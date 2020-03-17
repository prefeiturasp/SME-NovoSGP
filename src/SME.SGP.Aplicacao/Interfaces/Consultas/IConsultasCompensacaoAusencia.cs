﻿using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasCompensacaoAusencia
    {
        Task<PaginacaoResultadoDto<CompensacaoAusenciaListagemDto>> ListarPaginado(string turmaId, string disciplinaId, int bimestre, string nomeAtividade, string nomeAluno);
        Task<CompensacaoAusenciaCompletoDto> ObterPorId(long id);
        Task<IEnumerable<TurmaRetornoDto>> ObterTurmasParaCopia(string turmaOrigemId);
    }
}
