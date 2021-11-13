using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtividadeInfantil : RepositorioBase<AtividadeInfantil>, IRepositorioAtividadeInfantil
    {
        public RepositorioAtividadeInfantil(ISgpContext context) : base(context)
        {
        }

        public Task<AtividadeInfantil> ObterPorAtividadeClassroomId(long atividadeClassroomId)
        {
            var query = @"select * from atividade_infantil where aviso_classroom_id = @avisoClassroomId";
            return database.Conexao.QueryFirstOrDefaultAsync<AtividadeInfantil>(query, new { atividadeClassroomId });
        }
    }
}
