using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPainelEducacionalConsultaIndicadoresPap : IRepositorioPainelEducacionalConsultaIndicadoresPap
    {
        private readonly ISgpContextConsultas database;
        private const string COLUNAS_SELECT = @"tipo_pap tipoPap,
                                                total_turmas totalTurmas,
                                                total_alunos totalAlunos,
                                                total_alunos_com_frequencia_inferior_limite totalAlunosComFrequenciaInferiorLimite,
                                                total_alunos_dificuldade_top_1 totalAlunosDificuldadeTop1,
                                                total_alunos_dificuldade_top_2 totalAlunosDificuldadeTop2,
                                                total_alunos_dificuldade_outras totalAlunosDificuldadeOutras,
                                                nome_dificuldade_top_1 nomeDificuldadeTop1,
                                                nome_dificuldade_top_2 nomeDificuldadeTop2";

        public RepositorioPainelEducacionalConsultaIndicadoresPap(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoPapBase>> ObterConsolidacoesDrePorAno(int ano, string codigoDre)
        {
            var query = $@"select {COLUNAS_SELECT} 
                             from painel_educacional_consolidacao_pap_dre 
                            where ano_letivo = @ano and codigo_dre = @codigoDre";
            return await database.QueryAsync<PainelEducacionalConsolidacaoPapBase>(query, new { ano, codigoDre });
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoPapBase>> ObterConsolidacoesSmePorAno(int ano)
        {
            var query = $@"select {COLUNAS_SELECT} 
                             from painel_educacional_consolidacao_pap_sme 
                            where ano_letivo = @ano";
            return await database.QueryAsync<PainelEducacionalConsolidacaoPapBase>(query, new { ano });
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoPapBase>> ObterConsolidacoesUePorAno(int ano, string codigoDre, string codigoUe)
        {
            var query = $@"select {COLUNAS_SELECT} 
                             from painel_educacional_consolidacao_pap_ue 
                            where ano_letivo = @ano and codigo_dre = @codigoDre and codigo_ue = @codigoUe";
            return await database.QueryAsync<PainelEducacionalConsolidacaoPapBase>(query, new { ano, codigoDre, codigoUe });
        }
    }
}