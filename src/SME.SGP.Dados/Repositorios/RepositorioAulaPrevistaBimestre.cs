using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAulaPrevistaBimestre : RepositorioBase<AulaPrevistaBimestre>, IRepositorioAulaPrevistaBimestre
    {
        public RepositorioAulaPrevistaBimestre(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<IEnumerable<AulaPrevistaBimestreQuantidade>> ObterBimestresAulasPrevistasPorFiltro(long tipoCalendarioId, string turmaId, string disciplinaId)
        {
            StringBuilder query = new StringBuilder();

            query.Append(MontarSelect());
            query.Append(@" where tp.situacao and not tp.excluido and
                        p.tipo_calendario_id = @tipoCalendarioId and
                        ap.turma_id = @turmaId and
                        ap.disciplina_id = @disciplinaId ");
            query.Append(MontarGroupOrderBy());

            return (await database.Conexao.QueryAsync<AulaPrevistaBimestreQuantidade>(query.ToString(), new { tipoCalendarioId, turmaId, disciplinaId }));
        }

        public async Task<IEnumerable<AulaPrevistaBimestreQuantidade>> ObterBimestresAulasPrevistasPorId(long? aulaPrevistaId)
        {
            StringBuilder query = new StringBuilder();

            query.Append(MontarSelect());
            query.Append(@" where tp.situacao and not tp.excluido and
                        ap.id = @aulaPrevistaId ");
            query.Append(MontarGroupOrderBy());

            return (await database.Conexao.QueryAsync<AulaPrevistaBimestreQuantidade>(query.ToString(), new { aulaPrevistaId }));
        }

        private string MontarGroupOrderBy()
        {
            return @" group by p.bimestre, p.periodo_inicio, p.periodo_fim, apb.aulas_previstas, apb.Id,
                         	   ap.criado_em, ap.criado_por, ap.alterado_em , ap.alterado_por,
                               ap.alterado_rf, ap.criado_rf; ";
        }

        private string MontarSelect()
        {
            return @"select apb.id, apb.criado_em as CriadoEm, apb.criado_por as CriadoPor, apb.alterado_em as AlteradoEm, apb.alterado_por as AlteradoPor,
                         apb.alterado_rf as AlteradoRF, apb.criado_rf as CriadoRF, p.bimestre, p.periodo_inicio as inicio, p.periodo_fim as fim,
                         apb.aulas_previstas as Previstas,
                         SUM(a.quantidade) filter (where a.tipo_aula = 1 and a.aula_cj = false) as CriadasTitular,
                         SUM(a.quantidade) filter (where a.tipo_aula = 1 and a.aula_cj = true) as CriadasCJ,
                         SUM(a.quantidade) filter (where a.tipo_aula = 1 and rf.id is not null) as Cumpridas,
                         SUM(a.quantidade) filter (where a.tipo_aula = 2 and rf.id is not null) as Reposicoes
                         from periodo_escolar p
                         inner join tipo_calendario tp on p.tipo_calendario_id = tp.id
                         left join aula_prevista ap on ap.tipo_calendario_id = p.tipo_calendario_id
                         left join aula_prevista_bimestre apb on ap.id = apb.aula_prevista_id and p.bimestre = apb.bimestre
                         left join aula a on a.turma_id = ap.turma_id and
                         				a.disciplina_id = ap.disciplina_id and
			                            a.tipo_calendario_id = p.tipo_calendario_id and
				                        a.data_aula BETWEEN p.periodo_inicio AND p.periodo_fim
                                        and (a.id is null or not a.excluido)
                         left join registro_frequencia rf on a.id = rf.aula_id ";
        }
    }
}