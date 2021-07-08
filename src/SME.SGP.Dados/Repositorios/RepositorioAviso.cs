using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAviso : RepositorioBase<Aviso>, IRepositorioAviso
    {
        public RepositorioAviso(ISgpContext context) : base(context)
        {
        }

        public async Task<Aviso> ObterPorClassroomId(long avisoClassroomId)
        {
            var query = @"selec * from aviso where aviso_classroom_id = @avisoClassroomId";
            return await database.Conexao.QueryFirstOrDefaultAsync<Aviso>(query, new { avisoClassroomId });
        }
    }
}
