﻿using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConsolidacaoFrequenciaAlunoMensal
    {
        Task<long> Inserir(ConsolidacaoFrequenciaAlunoMensal consolidacao);
        Task LimparConsolidacaoFrequenciasAlunosPorTurmasEMeses(long[] turmasIds, int[] meses);
        Task<IEnumerable<ConsolidacaoFrequenciaAlunoMensalDto>> ObterConsolidacoesFrequenciaAlunoMensalPorTurmaEMes(long turmaId, int mes);
        Task AlterarConsolidacaoAluno(long consolidacaoId, decimal percentual, int quantidadeAulas, int quantidadeAusencias, int quantidadeCompensacoes);
        Task RemoverConsolidacaoAluno(long consolidacaoId);
    }
}
