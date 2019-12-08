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

        public async Task<IEnumerable<AulasPrevistasDadasDto>> ObterAulaPrevistaDada(long tipoCalendarioId, string turmaId, string disciplinaId)
        {
            var query = @"select p.bimestre, p.periodo_inicio as inicio, p.periodo_fim as fim, ap.Id as PD, ap.aulas_previstas as Quantidade,
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

            var aulasDadasPrevistas = new List<AulasPrevistasDadasDto>();

            return await database.Conexao.QueryAsync<AulasPrevistasDadasDto, AulasPrevistasDto, AulasQuantidadePorProfessorDto, AulasQuantidadePorProfessorDto, AulasPrevistasDadasDto>(query,
            (pd, previstas, criadas, cumpridas) =>
            {
                if (pd != null)
                {
                    pd.Criadas = criadas;
                    pd.Cumpridas = cumpridas;
                    pd.Previstas = previstas;

                    pd.Previstas.TemDivergencia = previstas.Quantidade != (criadas.QuantidadeCJ + criadas.QuantidadeTitular) ||
                                                  previstas.Quantidade != (cumpridas.QuantidadeCJ + cumpridas.QuantidadeTitular + pd.Reposicoes);
                }

                aulasDadasPrevistas.Add(pd);

                return pd;

            }, new
            {
                tipoCalendarioId,
                turmaId,
                disciplinaId
            }, splitOn: "bimestre,PD,QuantidadeTitular,QuantidadeTitular");
        }

        public async Task<IEnumerable<AulaPrevista>> ObterAulasPrevistasPorFiltro(int bimestre, long tipoCalendarioId, string turmaId, string disciplinaId)
        {
            try
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
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
