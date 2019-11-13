using Dapper;
using SME.SGP.Dados.Contexto;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAula : RepositorioBase<Aula>, IRepositorioAula
    {
        public RepositorioAula(ISgpContext conexao) : base(conexao)
        {
        }

        public bool UsuarioPodeCriarAulaNaUeETurma(Aula aula)
        {
            var query = "select 1 from v_abrangencia where turma_id = @turmaId and ue_codigo = @ueId";
            return database.Conexao.QueryFirst<bool>(query, new
            {
                aula.TurmaId,
                aula.UeId
            });
        }
    }
}