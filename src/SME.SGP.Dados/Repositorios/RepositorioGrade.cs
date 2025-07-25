using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using SME.SGP.Infra.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioGrade : RepositorioBase<Grade>, IRepositorioGrade
    {
        public RepositorioGrade(ISgpContextConsultas conexao, IServicoAuditoria servicoAuditoria) : base(conexao, servicoAuditoria)
        {
        }

        public async Task<Grade> ObterGradeTurma(TipoEscola tipoEscola, Modalidade modalidade, int duracao)
        {
            string query = @"select f.id as FiltroId, g.Id as GradeId, g.*
                  from grade_filtro f
                 inner join grade g on g.id = f.grade_id
                 where f.tipo_escola = @tipoEscola
                   and f.modalidade = @modalidade
                   and f.duracao_turno = @duracao";

            var filtro = await database.Conexao.QueryAsync<GradeFiltro, Grade, Grade>(query,
                (gradeFiltro, grade) =>
                {
                    return grade;
                }, new
                {
                    tipoEscola,
                    modalidade,
                    duracao
                }, splitOn: "FiltroId, GradeId");

            return filtro.FirstOrDefault();
        }

        public async Task<Grade> ObterGradeTurmaAno(TipoEscola tipoEscola, Modalidade modalidade, int duracao, int ano, string anoLetivo)
        {
            string query = @"select f.id as FiltroId, g.Id as GradeId, g.*
                  from grade_filtro f
                 inner join grade g on g.id = f.grade_id ";

            if (ano > 0)
                query += " inner join grade_disciplina gd on g.id = gd.grade_id ";

            query += @" where f.tipo_escola = @tipoEscola
                   and f.modalidade = @modalidade
                   and f.duracao_turno = @duracao";

            if (ano > 0)
                query += " and gd.ano = @ano ";

            if (modalidade == Modalidade.Medio)
                query += " and to_char(g.inicio_vigencia, 'YYYY') <= @anoLetivo and (to_char(g.fim_vigencia, 'YYYY') >= @anoLetivo or g.fim_vigencia is null) ";


            var filtro = await database.Conexao.QueryAsync<GradeFiltro, Grade, Grade>(query,
            (gradeFiltro, grade) =>
            {
                return grade;
            }, new
            {
                tipoEscola,
                modalidade,
                duracao,
                ano,
                anoLetivo
            }, splitOn: "FiltroId, GradeId");

            return filtro.FirstOrDefault();
        }

        public async Task<int> ObterHorasComponente(long gradeId, long[] componentesCurriculares, int ano)
        {
            var query = @"select gd.quantidade_aulas
                      from grade_disciplina gd
                     where gd.grade_id = @gradeId
                       and gd.componente_curricular_id = any(@componentesCurriculares)
                       and gd.ano = @ano";

            var consulta = await database.Conexao.QueryAsync<int>(query, new
            {
                gradeId,
                componentesCurriculares,
                ano
            });

            return consulta.Any() ? consulta.Single() : 0;
        }
    }
}