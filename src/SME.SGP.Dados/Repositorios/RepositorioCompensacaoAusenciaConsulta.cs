using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados
{
    public class RepositorioCompensacaoAusenciaConsulta : RepositorioBase<CompensacaoAusencia>, IRepositorioCompensacaoAusenciaConsulta
    {
        public RepositorioCompensacaoAusenciaConsulta(ISgpContextConsultas database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<PaginacaoResultadoDto<CompensacaoAusencia>> Listar(Paginacao paginacao, string turmaId, string[] disciplinasId, int bimestre, string nomeAtividade, string professor = null)
        {
            var retorno = new PaginacaoResultadoDto<CompensacaoAusencia>();

            var query = new StringBuilder(MontaQuery(paginacao, disciplinasId, bimestre, nomeAtividade, professor: professor));
            query.AppendLine(";");
            query.AppendLine(MontaQuery(paginacao, disciplinasId, bimestre, nomeAtividade, true, professor));

            var nomeAtividadeTratado = $"%{(nomeAtividade ?? "").ToLower()}%";

            using (var multi = await database.Conexao.QueryMultipleAsync(query.ToString(), new { turmaId, disciplinasId, bimestre, nomeAtividade = nomeAtividadeTratado, professor }))
            {
                retorno.Items = multi.Read<CompensacaoAusencia>().ToList();
                retorno.TotalRegistros = multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros);

            return retorno;
        }

        private string MontaQuery(Paginacao paginacao, string[] disciplinasId, int bimestre, string nomeAtividade, bool contador = false, string professor = null)
        {
            var select = contador ? "count(c.id)" : "c.id, c.bimestre, c.nome";
            var query = new StringBuilder(string.Format(@"select {0}
                          from compensacao_ausencia c
                         inner join turma t on t.id = c.turma_id
                          where not c.excluido  
                            and t.turma_id = @turmaId ", select));

            if (disciplinasId.NaoEhNulo() && disciplinasId.Any())
                query.AppendLine("and c.disciplina_id = any(@disciplinasId)");
            if (bimestre != 0)
                query.AppendLine("and c.bimestre = @bimestre");
            if (!string.IsNullOrEmpty(nomeAtividade))
                query.AppendLine("and lower(f_unaccent(c.nome)) like @nomeAtividade");
            if (!string.IsNullOrWhiteSpace(professor))
                query.AppendLine("and (c.professor_rf = @professor or c.professor_rf is null)");

            if (paginacao.QuantidadeRegistros > 0 && !contador)
                query.AppendLine($"OFFSET {paginacao.QuantidadeRegistrosIgnorados} ROWS FETCH NEXT {paginacao.QuantidadeRegistros} ROWS ONLY");

            return query.ToString();
        }

        public async Task<CompensacaoAusencia> ObterPorAnoTurmaENome(int anoLetivo, long turmaId, string nome, long idIgnorar, string[] disciplinasId, string professor = null)
        {
            var query = new StringBuilder(@"select * 
                                              from compensacao_ausencia c
                                             where not excluido
                                               and ano_letivo = @anoLetivo
                                               and turma_id = @turmaId
                                               and disciplina_id = any(@disciplinasId)
                                               and nome = @nome ");
            if (idIgnorar > 0)
                query.AppendLine("and id <> @idIgnorar ");

            if (!string.IsNullOrWhiteSpace(professor))
                query.AppendLine("and professor_rf = @professor ");

            return await database.Conexao.QueryFirstOrDefaultAsync<CompensacaoAusencia>(query.ToString(), new { anoLetivo, turmaId, nome, idIgnorar, disciplinasId, professor });
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
                                    inner join ue on
	                                    ue.id = t.ue_id
                                    inner join dre on
	                                    dre.id = ue.dre_id                                   
                                    inner join periodo_escolar pe on
                                        a.tipo_calendario_id = pe.tipo_calendario_id 
                                    left join componente_curricular cc on
	                                    cc.id = cast(a.disciplina_id as bigint)
                                    where t.ano_letivo = @anoLetivo and not a.excluido and coalesce(cc.permite_registro_frequencia, true) ");

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

        public async Task<IEnumerable<CompensacaoDataAlunoDto>> ObterAusenciaParaCompensacaoPorAlunos(long compensacaoAusenciaId, string[] codigosAlunos, string[] disciplinasId, int bimestre, string turmacodigo, string professor = null)
        {
            var query = @$"select
							    caaa.id  as CompensacaoAusenciaAlunoAulaId,
								rfa.aula_id as AulaId,
								caaa.data_aula as DataAula,
								'Aula ' || caaa.numero_aula as Descricao,
								caaa.registro_frequencia_aluno_id as RegistroFrequenciaAlunoId,
								rfa.codigo_aluno as CodigoAluno
							from compensacao_ausencia_aluno_aula caaa
							join compensacao_ausencia_aluno caa on caaa.compensacao_ausencia_aluno_id = caa.id
							join registro_frequencia_aluno rfa on rfa.id = caaa.registro_frequencia_aluno_id
							join aula a on a.id = rfa.aula_id
							inner join periodo_escolar p on a.tipo_calendario_id = p.tipo_calendario_id
							where not caaa.excluido 
							    and rfa.codigo_aluno = any(@codigosAlunos)
								and a.disciplina_id = any(@disciplinasId)
								and p.bimestre = @bimestre
								and a.turma_id = @turmacodigo
								and rfa.valor = 2
								and caa.compensacao_ausencia_id = @compensacaoAusenciaId
								{(!string.IsNullOrWhiteSpace(professor) ? " and a.professor_rf = @professor " : string.Empty)}
								order by caaa.data_aula ";

            var parametros = new { compensacaoAusenciaId, codigosAlunos, disciplinasId, bimestre, turmacodigo, professor };
            return await database.Conexao.QueryAsync<CompensacaoDataAlunoDto>(query, parametros);
        }
    }
}
