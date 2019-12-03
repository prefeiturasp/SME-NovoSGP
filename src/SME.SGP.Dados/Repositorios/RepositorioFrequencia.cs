using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequencia : RepositorioBase<RegistroFrequencia>, IRepositorioFrequencia
    {
        public RepositorioFrequencia(ISgpContext database) : base(database)
        {
        }

        public async Task ExcluirFrequenciaAula(long aulaId)
        {
            // Exclui registros de ausencia do aluno
            var command = @"update registro_ausencia_aluno 
                                set excluido = true
                            where not excluido 
                              and registro_frequencia_id in (
                                select id from registro_frequencia 
                                 where not excluido 
                                   and aula_id = @aulaId)";
            await database.ExecuteAsync(command, new { aulaId });

            // Exclui registro de frequencia da aula
            command = @"update registro_frequencia 
                            set excluido = true 
                        where not excluido 
                          and aula_id = @aulaId ";
            await database.ExecuteAsync(command, new { aulaId });
        }

        public IEnumerable<RegistroAusenciaAluno> ObterListaFrequenciaPorAula(long aulaId)
        {
            var query = @"select ra.*
                        from
	                        registro_frequencia rf
                        inner join registro_ausencia_aluno ra on
	                        rf.id = ra.registro_frequencia_id
                        inner join aula a on
	                        a.id = rf.aula_id
                        where ra.excluido = false and
	                        a.id = @aulaId";

            return database.Conexao.Query<RegistroAusenciaAluno>(query, new { aulaId });
        }

        public RegistroFrequencia ObterRegistroFrequenciaPorAulaId(long aulaId)
        {
            var query = @"select *
                            from registro_frequencia
                          where not excluido 
                            and aula_id = @aulaId";

            return database.Conexao.QueryFirstOrDefault<RegistroFrequencia>(query, new { aulaId });
        }

        public IEnumerable<RegistroFrequenciaFaltanteDto> ObterTurmasSemRegistroDeFrequencia()
        {
            var query = @"select a.turma_id as CodigoTurma, t.nome as NomeTurma, a.disciplina_id as DisciplinaId
	                        , ue.ue_id as CodigoUe, ue.nome as NomeUe
	                        , dre.dre_id as CodigoDre, dre.nome as NomeDre
	                        , count(a.id) as QuantidadeAulasSemFrequencia
	                        , string_agg(cast(a.id as varchar), ',') as aulas
                          from aula a
                          inner join turma t on t.turma_id = a.turma_id
                          inner join ue on ue.id = t.ue_id
                          inner join dre on dre.id = ue.dre_id
                          left join registro_frequencia r on r.aula_id = a.id
                         where not a.excluido
                           and r.id is null
                           and a.data_aula < DATE(now())
                        group by a.turma_id, t.Nome, a.disciplina_id, ue.ue_id, ue.nome, dre.dre_id, dre.nome
                        order by dre.dre_id, ue.ue_id, a.turma_id";

            return database.Conexao.Query<RegistroFrequenciaFaltanteDto>(query);
        }
    }
}