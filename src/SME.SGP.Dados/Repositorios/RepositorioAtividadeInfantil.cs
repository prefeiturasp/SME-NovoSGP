using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtividadeInfantil : RepositorioBase<AtividadeInfantil>, IRepositorioAtividadeInfantil
    {
        public RepositorioAtividadeInfantil(ISgpContext context) : base(context) {}
        public async Task<IEnumerable<AtividadeInfantilDto>> ObterPorAulaId(long aulaId)
        {
            var query = @"select id
                            , titulo
                            , mensagem
                            , email 
                         from atividade_infantil 
                        where aula_id = @aulaId";

            return await database.Conexao.QueryAsync<AtividadeInfantilDto>(query, new { aulaId });
        }

        public async Task<AtividadeInfantil> ObterPorAtividadeClassroomId(long atividadeClassroomId)
        {
            var query = @"select * from atividade_infantil where aviso_classroom_id = @avisoClassroomId";
            return await database.Conexao.QueryFirstOrDefaultAsync<AtividadeInfantil>(query, new { atividadeClassroomId });
        }
    }
}
