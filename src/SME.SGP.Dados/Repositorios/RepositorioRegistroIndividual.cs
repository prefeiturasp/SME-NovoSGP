using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroIndividual : RepositorioBase<RegistroIndividual>, IRepositorioRegistroIndividual
    {
        public RepositorioRegistroIndividual(ISgpContext database) : base(database)
        {
        }

        public async Task<RegistroIndividual> ObterPorAlunoData(long turmaId, long componenteCurricularId, long alunoCodigo, DateTime data)
        {
            var query = @"select id,
	                            turma_id,
	                            aluno_codigo,
	                            componente_curricular_id,
	                            data_registro,
	                            registro,
	                            criado_em,
	                            criado_por,
	                            criado_rf,
	                            alterado_em,
	                            alterado_por,
	                            alterado_rf,
	                            excluido,
	                            migrado
                        from registro_individual 
                       where not excluido 
                        and turma_id = @turmaId
                        and componente_curricular_id = @componenteCurricularId
                        and aluno_codigo = @alunoCodigo
                        and data_registro::date = @data ";

            return await database.Conexao.QueryFirstOrDefaultAsync<RegistroIndividual>(query, new { turmaId, componenteCurricularId, alunoCodigo, data });
        }

        public async Task<PaginacaoResultadoDto<RegistroIndividual>> ObterPorAlunoPeriodoPaginado(long turmaId, long componenteCurricularId, long alunoCodigo, DateTime dataInicio, DateTime dataFim, Paginacao paginacao)
        {
            var condicao = @" from registro_individual 
                       where not excluido 
                        and turma_id = @turmaId
                        and componente_curricular_id = @componenteCurricularId
                        and aluno_codigo = @alunoCodigo
                        and data_registro::date between @dataInicio and @dataFim ";
            var orderBy = "order by data_registro desc";

            if (paginacao == null || (paginacao.QuantidadeRegistros == 0 && paginacao.QuantidadeRegistrosIgnorados == 0))
                paginacao = new Paginacao(1, 10);

            var query = $"select count(0) {condicao}";

            var totalRegistrosDaQuery = await database.Conexao.QueryFirstOrDefaultAsync<int>(query,
                new { turmaId, componenteCurricularId, alunoCodigo, dataInicio, dataFim });

            var offSet = "offset @qtdeRegistrosIgnorados rows fetch next @qtdeRegistros rows only";

            query = $@"select id,
                              turma_id,
	                          aluno_codigo,
	                          componente_curricular_id,
	                          data_registro,
	                          registro,
	                          criado_em,
	                          criado_por,
	                          criado_rf,
	                          alterado_em,
	                          alterado_por,
	                          alterado_rf,
	                          excluido,
	                          migrado {condicao} {orderBy} {offSet} ";

            return new PaginacaoResultadoDto<RegistroIndividual>()
            {
                Items = await database.Conexao.QueryAsync<RegistroIndividual>(query,
                                                  new
                                                  {
                                                      turmaId,
                                                      componenteCurricularId,
                                                      alunoCodigo,
                                                      dataInicio,
                                                      dataFim,
                                                      qtdeRegistrosIgnorados = paginacao.QuantidadeRegistrosIgnorados,
                                                      qtdeRegistros = paginacao.QuantidadeRegistros
                                                  }),
                TotalRegistros = totalRegistrosDaQuery,
                TotalPaginas = (int)Math.Ceiling((double)totalRegistrosDaQuery / paginacao.QuantidadeRegistros)
            };
        }

        public async Task<IEnumerable<UltimoRegistroIndividualAlunoTurmaDto>> ObterUltimosRegistrosPorAlunoTurma(long turmaId)
        {
            const string query = @"select  
                            turma_id AS TurmaId,
                            aluno_codigo AS CodigoAluno,
                            max(data_registro) AS DataRegistro
                        from 
                            public.registro_individual 
                        where 
                            turma_id = @turmaId
                        and 
                            not excluido
                        group by 
	                        turma_id , aluno_codigo ";

            return await database.Conexao.QueryAsync<UltimoRegistroIndividualAlunoTurmaDto>(query, new { turmaId });
        }
        public async Task<SugestaoTopicoRegistroIndividualDto> ObterSugestaoTopicoPorMes(int mes)
        {
            const string query = @"select ris.id,
                                          ris.descricao 
                                     from registro_individual_sugestao ris 
                                    where ris.mes = @mes
                                      and not excluido";

            return await database.Conexao.QueryFirstOrDefaultAsync<SugestaoTopicoRegistroIndividualDto>(query, new { mes });
        }

        public async Task<IEnumerable<QuantidadeRegistrosIndividuaisPorAnoTurmaDTO>> ObterQuantidadeRegistrosIndividuaisPorAnoTurmaAsync(int anoLetivo, long dreId, long ueId, Modalidade modalidade)
        {
            var sql = @"";
            if (ueId == 0)
            {
                sql = @" select  
                            t.ano,
                            count(ri.id) as quantidadeRegistrosIndividuais
                        from registro_individual ri
                        inner join turma t on ri.turma_id = t.id  
                            inner join ue on ue.id = t.ue_id
                            inner join dre on dre.id = ue.dre_id
                        where not ri.excluido
                            and t.ano_letivo = @anoLetivo
                        group by t.ano ";
            }
            else
            {
                sql = @" select
                            t.nome as turma,
                            count(ri.id) as quantidadeRegistrosIndividuais
                        from registro_individual ri
                        inner join turma t on ri.turma_id = t.id
                        inner join ue on ue.id = t.ue_id
                        inner join dre on dre.id = ue.dre_id
                        where not ri.excluido
                            and t.ano_letivo = @anoLetivo
                            and dre.id = @dreId
	                        and t.ue_id = @ueId
                        group by t.nome ";
            }

            return await database.Conexao.QueryAsync<QuantidadeRegistrosIndividuaisPorAnoTurmaDTO>(sql, new { anoLetivo, dreId, ueId, modalidade });
        }

        public async Task<IEnumerable<RegistroIndividualDTO>> ObterTurmasComRegistrosIndividuaisInfantilEAnoAsync(int anoLetivo)
        {
            var query = @" select 
                                distinct
	                            ri.turma_id as turmaId, 
	                            tu.ano_letivo as anoLetivo
                            from registro_individual ri 
                            inner join turma tu on ri.turma_id = tu.id 
                            where not ri.excluido 
	                            and tu.ano_letivo = @anoLetivo
	                            and tu.modalidade_codigo in (1,2)
                            order by ri.turma_id ";

            return await database.Conexao.QueryAsync<RegistroIndividualDTO>(query, new { anoLetivo });
        }

        public async Task<IEnumerable<AlunoInfantilComRegistroIndividualDTO>> ObterAlunosInfantilComRegistrosIndividuaisPorTurmaAnoAsync(long turmaCodigo, int anoLetivo)
        {
            var query = @" select 
                                distinct
                                tu.id as turmaId,
	                            ri.aluno_codigo as AlunoCodigo
                            from registro_individual ri 
                            inner join turma tu on ri.turma_id = tu.id 
                            where not ri.excluido 
	                            and tu.ano_letivo = @anoLetivo
                                and ri.turma_id = @turmaCodigo
	                            and tu.modalidade_codigo in (1,2)
                            order by tu.id ";

            return await database.Conexao.QueryAsync<AlunoInfantilComRegistroIndividualDTO>(query, new { turmaCodigo, anoLetivo });
        }

        public async Task<IEnumerable<RegistroIndividualAlunoDTO>> ObterRegistrosIndividuaisPorTurmaAlunoAsync(long turmaCodigo, long codigoAluno)
        {
            var query = @" select 
                                distinct
	                            ri.aluno_codigo as AlunoCodigo,
                                ri.data_registro as DataRegistro
                            from registro_individual ri 
                            inner join turma tu on ri.turma_id = tu.id 
                            where not ri.excluido 
                                and ri.turma_id = @turmaCodigo
                                and ri.aluno_codigo = @codigoAluno
	                            and tu.modalidade_codigo in (1,2)
                            order by ri.data_registro ";

            return await database.Conexao.QueryAsync<RegistroIndividualAlunoDTO>(query, new { turmaCodigo, codigoAluno });
        }
    }
}
