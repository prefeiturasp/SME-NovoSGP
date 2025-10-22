using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Consts;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    [ExcludeFromCodeCoverage]
    public class RepositorioPainelEducacionalConsolidacaoNotaConsulta : IRepositorioPainelEducacionalConsolidacaoNotaConsulta
    {
        private readonly ISgpContextConsultas database;
        public RepositorioPainelEducacionalConsolidacaoNotaConsulta(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoNotaDadosBrutos>> ObterDadosBrutosPorAnoLetivoAsync(int anoLetivo)
        {
            var query = @$"
                select turma.nome as turmaNome,
                       turma.ano as anoTurma,
                       turma.modalidade_codigo as modalidade,
                       turma.ano_letivo as anoLetivo,
                       ue.ue_id as codigoUe,
                       dre.dre_id as codigoDre,
                       cccNota.componente_curricular_id as idComponenteCurricular,
                       cccNota.bimestre,
                       cccNota.nota,
                       conceitos.valor as valorConceito,
                       conceitos.aprovado as conceitoDeAprovado,
                       (select valor_medio 
                         from notas_parametros notasParametros 
                        where cccAlunoTurma.dt_atualizacao between notasParametros.inicio_vigencia and 
                                                                   notasParametros.fim_vigencia 
                           or (cccAlunoTurma.dt_atualizacao > notasParametros.inicio_vigencia and 
                               notasParametros.fim_vigencia is null)
                        order by notasParametros.inicio_vigencia  desc
                        limit 1) as valorMedioNota
                  from consolidado_conselho_classe_aluno_turma cccAlunoTurma 
	                   inner join turma on turma.id = cccAlunoTurma.turma_id
	                   inner join ue on ue.id = turma.ue_id 
	                   inner join dre on dre.id = ue.dre_id
  	                   inner join consolidado_conselho_classe_aluno_turma_nota cccNota on cccNota.consolidado_conselho_classe_aluno_turma_id = cccAlunoTurma.id 
	                    left join conceito_valores conceitos on conceitos.id = cccNota.conceito_id
                 where cccNota.componente_curricular_id in ({string.Join(',', PainelEducacionalConstants.ComponentesCurricularesConsolidacaoNotas)})
                   and turma.ano_letivo = @anoLetivo
                 order by dre.dre_id, ue.ue_id, cccNota.bimestre, turma.modalidade_codigo, turma.turma_id";

            return await database.QueryAsync<PainelEducacionalConsolidacaoNotaDadosBrutos>(query, new { anoLetivo }, commandTimeout: 600);
        }

        public async Task<int?> ObterUltimoAnoConsolidadoAsync()
        {
            const string query = @"select max(ano_letivo) from painel_educacional_consolidacao_nota";
            return await database.QueryFirstOrDefaultAsync<int?>(query);
        }
    }
}