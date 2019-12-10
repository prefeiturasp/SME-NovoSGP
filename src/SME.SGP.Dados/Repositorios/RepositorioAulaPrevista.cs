using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAulaPrevista : RepositorioBase<AulaPrevista>, IRepositorioAulaPrevista
    {
        public RepositorioAulaPrevista(ISgpContext conexao) : base(conexao)
        {
        }

        public async Task<AulasPrevistasDadasAuditoriaDto> ObterAulaPrevistaDada(long tipoCalendarioId, string turmaId, string disciplinaId)
        {
            var query = @"select ap.criado_em as CriadoEm, ap.criado_por as CriadoPor, ap.alterado_em as AlteradoEm, ap.alterado_por as AlteradoPor,
                        p.bimestre, p.periodo_inicio as inicio, p.periodo_fim as fim, ap.Id as PD, ap.aulas_previstas as Quantidade,
                         COUNT(a.id) filter (where a.tipo_aula = 1 and a.aula_cj = false) as QuantidadeTitular,
                         COUNT(a.id) filter (where a.tipo_aula = 1 and a.aula_cj = true) as QuantidadeCJ,
                         COUNT(a.id) filter (where a.tipo_aula = 1 and rf.id is not null and a.aula_cj = false) as QuantidadeTitular,
                         COUNT(a.id) filter (where a.tipo_aula = 1 and rf.id is not null and a.aula_cj = true) as QuantidadeCJ, 
                         COUNT(a.id) filter (where a.tipo_aula = 2 and rf.id is not null) as Reposicoes                         
                         from aula_prevista ap
                         right join periodo_escolar p on ap.tipo_calendario_id = p.tipo_calendario_id and ap.bimestre = p.bimestre
                         left join aula a on p.tipo_calendario_id = a.tipo_calendario_id and 
				                        a.data_aula BETWEEN p.periodo_inicio AND p.periodo_fim
                         left join registro_frequencia rf on a.id = rf.aula_id
                         where p.tipo_calendario_id = @tipoCalendarioId and (a.turma_id = @turmaId or a.turma_id is null) and
                               (a.disciplina_id = @disciplinaId or a.disciplina_id is null)
                         group by p.bimestre, p.periodo_inicio, p.periodo_fim, ap.aulas_previstas, ap.Id
                         order by p.periodo_inicio;";

            AulasPrevistasDadasAuditoriaDto aulasDadasPrevistasAuditoria = null;
            List<string> mensagens = null;

            var list = await database.Conexao.QueryAsync<AulasPrevistasDadasAuditoriaDto, AulasPrevistasDadasDto, AulasPrevistasDto, AulasQuantidadePorProfessorDto, AulasQuantidadePorProfessorDto, AulasPrevistasDadasAuditoriaDto>(query,
            (pda, pd, previstas, criadas, cumpridas) =>
            {
                if (pda != null)
                {
                    pd.Criadas = criadas;
                    pd.Cumpridas = cumpridas;
                    pd.Previstas = previstas;

                    mensagens = new List<string>();

                    if (previstas.Quantidade != (criadas.QuantidadeCJ + criadas.QuantidadeTitular))
                        mensagens.Add("Quantidade de aulas previstas diferente da quantidade de aulas criadas.");

                    if (previstas.Quantidade != (cumpridas.QuantidadeCJ + cumpridas.QuantidadeTitular + pd.Reposicoes))
                        mensagens.Add("Quantidade de aulas previstas diferente do somatório de aulas dadas + aulas repostas, após o final do bimestre.");

                    pd.Previstas.Mensagens = mensagens.ToArray();

                }

                if (aulasDadasPrevistasAuditoria == null)
                    aulasDadasPrevistasAuditoria = pda;

                if (aulasDadasPrevistasAuditoria.AulasPrevistasPorBimestre == null)
                    aulasDadasPrevistasAuditoria.AulasPrevistasPorBimestre = new List<AulasPrevistasDadasDto>();

                aulasDadasPrevistasAuditoria.AulasPrevistasPorBimestre.AsList().Add(pd);

                return pda;

            }, new
            {
                tipoCalendarioId,
                turmaId,
                disciplinaId
            }, splitOn: "CriadoEm,bimestre,PD,QuantidadeTitular,QuantidadeTitular");

            return aulasDadasPrevistasAuditoria;
        }

        public async Task<IEnumerable<AulaPrevista>> ObterAulasPrevistasPorFiltro(int bimestre, long tipoCalendarioId, string turmaId, string disciplinaId)
        {
            var query = new StringBuilder();

            query.AppendLine("select * from aula_prevista where 1=1");

            if (tipoCalendarioId > 0)
                query.AppendLine("and tipo_calendario_id = @tipoCalendarioId");

            if (bimestre > 0)
                query.AppendLine("and bimestre = @bimestre");

            if (!string.IsNullOrEmpty(turmaId))
                query.AppendLine("and turma_id = @turmaId");

            if (!string.IsNullOrEmpty(disciplinaId))
                query.AppendLine("and disciplina_id = @disciplinaId");

            return await database.Conexao.QueryAsync<AulaPrevista>(query.ToString(), new { bimestre, tipoCalendarioId, turmaId, disciplinaId });
        }

        public string ObterProfessorTurmaDisciplinaAulasPrevistasDivergente(int bimestre, string turmaId, string disciplinaId, int limiteDias)
        {
            var query = @"select a.professor_rf
                          from aula a
                         inner join periodo_escolar p on a.tipo_calendario_id = p.tipo_calendario_id and  a.data_aula BETWEEN p.periodo_inicio AND p.periodo_fim
                         where not a.excluido
                           and now() between p.periodo_inicio and p.periodo_fim
                           and DATE_PART('day', age(p.periodo_fim, date(now()))) <= @limiteDias
                           and p.bimestre = @bimestre
                           and a.turma_id = @turmaId
                           and a.disciplina_id = @disciplinaId
                          ORDER BY a.data_aula DESC NULLS LAST LIMIT 1";

            return database.Conexao.QuerySingleOrDefault<string>(query, new { bimestre, turmaId, disciplinaId, limiteDias });
        }
    }
}
