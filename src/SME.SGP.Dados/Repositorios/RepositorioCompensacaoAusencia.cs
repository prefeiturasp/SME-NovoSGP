using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioCompensacaoAusencia : RepositorioBase<CompensacaoAusencia>, IRepositorioCompensacaoAusencia
    {
        public RepositorioCompensacaoAusencia(ISgpContext database) : base(database)
        {
        }

        public async Task<PaginacaoResultadoDto<CompensacaoAusencia>> Listar(Paginacao paginacao, string turmaId, string disciplinaId, int bimestre, string nomeAtividade)
        {
            var retorno = new PaginacaoResultadoDto<CompensacaoAusencia>();

            var query = new StringBuilder(MontaQuery(paginacao, disciplinaId, bimestre, nomeAtividade));
            query.AppendLine(";");
            query.AppendLine(MontaQuery(paginacao, disciplinaId, bimestre, nomeAtividade, true));

            var nomeAtividadeTratado = $"%{(nomeAtividade ?? "").ToLower()}%";

            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), new { turmaId, disciplinaId, bimestre, nomeAtividade = nomeAtividadeTratado }))
            {
                retorno.Items = multi.Read<CompensacaoAusencia>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string MontaQuery(Paginacao paginacao, string disciplinaId, int bimestre, string nomeAtividade, bool contador = false)
        {
            var select = contador ? "count(c.id)" : "c.id, c.bimestre, c.nome";
            var query = new StringBuilder(string.Format(@"select {0}
                          from compensacao_ausencia c
                         inner join turma t on t.id = c.turma_id
                          where not c.excluido  
                            and t.turma_id = @turmaId ", select));

            if (!string.IsNullOrEmpty(disciplinaId))
                query.AppendLine("and c.disciplina_id = @disciplinaId");
            if (bimestre != 0)
                query.AppendLine("and c.bimestre = @bimestre");
            if (!string.IsNullOrEmpty(nomeAtividade))
                query.AppendLine("and lower(f_unaccent(c.nome)) like @nomeAtividade");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                query.AppendLine($"OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY");

            return query.ToString();
        }

        public async Task<CompensacaoAusencia> ObterPorAnoTurmaENome(int anoLetivo, long turmaId, string nome, long idIgnorar)
        {
            var query = @"select * 
                            from compensacao_ausencia c
                          where not excluido
                            and ano_letivo = @anoLetivo
                            and turma_id = @turmaId
                            and nome = @nome
                            and id <> @idIgnorar";

            return await database.Conexao.QueryFirstOrDefaultAsync<CompensacaoAusencia>(query, new { anoLetivo, turmaId, nome, idIgnorar });
        }

        public async Task<IEnumerable<Infra.TotalCompensacaoAusenciaDto>> ObterCompesacoesAusenciasConsolidadasPorTurmaEAno(int anoLetivo, long dreId, long ueId, int modalidade, int bimestre, int semestre)
        {
            var query = new StringBuilder(@"select ");

            if (ueId != -99)
                query.AppendLine("t.nome as DescricaoAnoTurma, ");
            else
                query.AppendLine("t.ano as DescricaoAnoTurma, ");

            query.AppendLine(@"t.modalidade_codigo as ModalidadeCodigo,
           		               sum(caa.qtd_faltas_compensadas) as Quantidade           		   
	                      from compensacao_ausencia_aluno caa 
	                     inner join compensacao_ausencia ca on ca.id = caa.compensacao_ausencia_id
	                     inner join turma t on t.id = ca.turma_id 
     		             inner join ue on ue.id = t.ue_id 
     		             inner join dre on dre.id = ue.dre_id 
     		             where t.ano_letivo = @anoLetivo
     		               and not caa.excluido
       		               and t.modalidade_codigo = @modalidade ");

            if (dreId != -99)
                query.AppendLine("and dre.id = @dreId ");

            if (ueId != -99)
                query.AppendLine("and ue.id = @ueId ");

            if (bimestre != -99 && bimestre > 0)
                query.AppendLine("and ca.bimestre = @bimestre ");

            if (semestre > 0)
                query.AppendLine("and t.semestre = @semestre ");

            if (ueId != -99)
                query.AppendLine("group by t.nome, t.modalidade_codigo");
            else
                query.AppendLine("group by t.ano, t.modalidade_codigo");

            var paramentros = new
            {
                dreId,
                ueId,
                anoLetivo,
                modalidade,
                semestre,
                bimestre
            };

            return await database.Conexao.QueryAsync<Infra.TotalCompensacaoAusenciaDto>(query.ToString(), paramentros);
        }

        public async Task<Infra.Dtos.TotalCompensacaoAusenciaDto> ObterTotalPorAno(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, int bimestre)
        {
            var query = new StringBuilder(@"select max(totalaulas) as totalaulas, max(totalcompensacoes) as totalcompensacoes from ");
            query.AppendLine(@"(select
	                                    t.ano_letivo as anoLetivo,
	                                    sum(0) as TotalAulas,
	                                    sum(caa.qtd_faltas_compensadas) as TotalCompensacoes
                                    from
	                                    compensacao_ausencia_aluno caa
                                    inner join compensacao_ausencia ca on
	                                    ca.id = caa.compensacao_ausencia_id
                                    inner join turma t on
	                                    t.id = ca.turma_id
                                    inner join ue on
	                                    ue.id = t.ue_id
                                    inner join dre on
	                                    dre.id = ue.dre_id
                                    where t.ano_letivo = @anoLetivo and not caa.excluido ");
            if (dreId != -99)
                query.AppendLine(" and dre.id = @dreId ");
            if (ueId != -99)
                query.AppendLine(" and ue.id = @ueId ");

            if (modalidade != 0)
                query.AppendLine(" and t.modalidade_codigo = @modalidade ");

            if (semestre != 0)
                query.AppendLine(" and t.semestre = @semestre ");

            if (bimestre > 0)
                query.AppendLine(" and ca.bimestre = @bimestre ");            

            query.AppendLine(@"group by t.ano_letivo 
                                    union
                                    select
	                                    t.ano_letivo as anoLetivo,
	                                    count(a.id) as TotalAulas,
	                                    sum(0) as TotalCompensacoes
                                    from
	                                    aula a
                                    inner join turma t on
	                                    a.turma_id = t.turma_id
                                    left join registro_frequencia rf on
	                                    a.id = rf.aula_id
                                    inner join componente_curricular cc on
	                                    cc.id = cast(a.disciplina_id as bigint)
                                    inner join ue on
	                                    ue.id = t.ue_id
                                    inner join dre on
	                                    dre.id = ue.dre_id                                   
                                    inner join periodo_escolar pe on
                                        a.tipo_calendario_id = pe.tipo_calendario_id 
                                    where t.ano_letivo = @anoLetivo and not a.excluido and cc.permite_registro_frequencia ");

            if (dreId != -99)
                query.AppendLine(" and dre.id = @dreId ");
            if (ueId != -99)
                query.AppendLine(" and ue.id = @ueId ");

            if (modalidade != 0)
                query.AppendLine(" and t.modalidade_codigo = @modalidade ");

            if (semestre != 0)
                query.AppendLine(" and t.semestre = @semestre ");

            if (bimestre > 0)
                query.AppendLine(" and pe.bimestre = @bimestre ");

            query.AppendLine(" group by t.ano_letivo) as query");

            return await database.Conexao.QueryFirstOrDefaultAsync<Infra.Dtos.TotalCompensacaoAusenciaDto>(query.ToString(), new { anoLetivo, dreId, ueId, modalidade, semestre, bimestre });                        
        }

        public async Task<IEnumerable<Infra.TotalCompensacaoAusenciaDto>> ObterAtividadesCompensacaoConsolidadasPorTurmaEAno(int anoLetivo, long dreId, long ueId, int modalidade, int bimestre, int semestre)
        {
            var query = new StringBuilder(@"select ");

            if (ueId != -99)
                query.AppendLine("t.nome as DescricaoAnoTurma, ");
            else
                query.AppendLine("t.ano as DescricaoAnoTurma, ");

            query.AppendLine(@"t.modalidade_codigo as ModalidadeCodigo,
           		               count(ca.id) as Quantidade FROM
                            compensacao_ausencia ca
	                     inner join turma t on t.id = ca.turma_id 
     		             inner join ue on ue.id = t.ue_id 
     		             inner join dre on dre.id = ue.dre_id 
     		             where t.ano_letivo = @anoLetivo
     		               and not ca.excluido
       		               and t.modalidade_codigo = @modalidade ");

            if (dreId != -99)
                query.AppendLine("and dre.id = @dreId ");

            if (ueId != -99)
                query.AppendLine("and ue.id = @ueId ");

            if (bimestre != -99 && bimestre > 0)
                query.AppendLine("and ca.bimestre = @bimestre ");

            if (semestre > 0)
                query.AppendLine("and t.semestre = @semestre ");

            if (ueId != -99)
                query.AppendLine("group by t.nome, t.modalidade_codigo");
            else
                query.AppendLine("group by t.ano, t.modalidade_codigo");

            var paramentros = new
            {
                dreId,
                ueId,
                anoLetivo,
                modalidade,
                semestre,
                bimestre
            };

            return await database.Conexao.QueryAsync<Infra.TotalCompensacaoAusenciaDto>(query.ToString(), paramentros);
        }
    }
}
