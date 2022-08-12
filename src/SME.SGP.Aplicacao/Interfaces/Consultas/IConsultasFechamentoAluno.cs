﻿using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasFechamentoAluno
    {
        Task<FechamentoAlunoCompletoDto> ObterAnotacaoAluno(string codigoAluno, long fechamentoId, string codigoTurma, int anoLetivo);
        Task<IEnumerable<FechamentoAlunoAnotacaoConselhoDto>> ObterAnotacaoAlunoParaConselhoAsync(string alunoCodigo, string[] turmasCodigos, long periodoId);
        Task<AnotacaoFechamentoAluno> ObterAnotacaoPorAlunoEFechamento(long fechamentoId, string codigoAluno);
    }
}