using System.Linq;
using System.Threading.Tasks;
using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioGrade : RepositorioBase<Grade>, IRepositorioGrade
    {
        public RepositorioGrade(ISgpContext conexao) : base(conexao)
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
    }
}
