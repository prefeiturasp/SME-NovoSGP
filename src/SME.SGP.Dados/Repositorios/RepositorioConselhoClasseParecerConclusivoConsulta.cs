using Dapper;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConselhoClasseParecerConclusivoConsulta : RepositorioBase<ConselhoClasseParecerConclusivo>, IRepositorioConselhoClasseParecerConclusivo
    {
        public RepositorioConselhoClasseParecerConclusivoConsulta(ISgpContextConsultas database) : base(database)
        {
        }

        public async Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaPorTurmaIdAsync(long turmaId, DateTime dataConsulta)
        {
            var where = "t.id = @parametro";

            return await ObterListaPorTurma(where, turmaId, dataConsulta);
        }

        public async Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaPorTurmaCodigoAsync(long turmaCodigo, DateTime dataConsulta)
        {
            var where = "t.turma_id = @parametro";

            return await ObterListaPorTurma(where, turmaCodigo, dataConsulta);
        }

        public async Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaVigente(DateTime dataConsulta)
        {
            var sql = @"select ccp.* from conselho_classe_parecer ccp 
                        where ccp.inicio_vigencia <= @dataConsulta and (ccp.fim_vigencia >= @dataConsulta or ccp.fim_vigencia is null)";

            var param = new { dataConsulta };

            return await database.Conexao.QueryAsync<ConselhoClasseParecerConclusivo>(sql, param);
        }

        private async Task<IEnumerable<ConselhoClasseParecerConclusivo>> ObterListaPorTurma(string where, object parametro, DateTime dataConsulta)
        {
            var sql = string.Format(ObterSqlParecerConclusivoTurma(), where);

            var param = new { parametro, dataConsulta };

            return await database.Conexao.QueryAsync<ConselhoClasseParecerConclusivo>(sql, param);
        }

        private string ObterSqlParecerConclusivoTurma()
        {
            return @"select ccp.* from conselho_classe_parecer ccp 
                        inner join conselho_classe_parecer_ano ccpa on ccp.id = ccpa.parecer_id 
                        inner join turma t on inner join turma t on ccpa.modalidade = t.modalidade_codigo 
	                                                                and (t.ano = 'S' and ccpa.ano_turma = 1 OR cast(ccpa.ano_turma as varchar) = t.ano) 
                        where {0} and ccpa.inicio_vigencia <= @dataConsulta and (ccpa.fim_vigencia >= @dataConsulta or ccpa.fim_vigencia is null)";
        }

        public async Task<IEnumerable<ParecerConclusivoSituacaoQuantidadeDto>> ObterParecerConclusivoSituacao(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre)
        {
            var query = new StringBuilder();
            query.AppendLine(@"SELECT t.turma_id as TurmaCodigo,              
                                      ccp.nome as Situacao,
                                      count(cca.id) AS Quantidade,
 			                          t.Nome AS AnoTurma,
                                      cca.aluno_codigo as AlunoCodigo
 		                         FROM conselho_classe_aluno cca
                                INNER JOIN conselho_classe_parecer ccp ON cca.conselho_classe_parecer_id = ccp.id
                                INNER JOIN conselho_classe cc ON cca.conselho_classe_id = cc.id
                                INNER JOIN fechamento_turma ft ON cc.fechamento_turma_id = ft.id
                                INNER JOIN turma t ON ft.turma_id = t.id
                                INNER JOIN ue u ON t.ue_id = u.id  
                                 LEFT JOIN periodo_escolar pe ON ft.periodo_escolar_id = pe.id
                                WHERE t.tipo_turma in (1) 
		                          AND t.ano_letivo = @ano ");

            if (ueId > 0)
                query.AppendLine(" AND t.ue_id = @ueId ");

            if (dreId > 0)
                query.AppendLine(" AND u.dre_id = @dreId ");

            if (modalidade > 0)
                query.AppendLine(" AND t.modalidade_codigo = @modalidade ");

            if (semestre > 0)
                query.AppendLine(" AND t.semestre = @semestre ");

            if (bimestre > 0)
                query.AppendLine(" AND pe.bimestre = @bimestre ");


            query.AppendLine(@" GROUP BY t.turma_id, ccp.nome , t.Nome, cca.aluno_codigo ");

            return await database.Conexao.QueryAsync<ParecerConclusivoSituacaoQuantidadeDto>(query.ToString(), new
            {
                ueId,
                ano,
                dreId,
                modalidade,
                semestre,
                bimestre
            });
        }
    }
}

