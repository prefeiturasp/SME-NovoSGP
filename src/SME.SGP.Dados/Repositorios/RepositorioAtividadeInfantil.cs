using System.Collections.Generic;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAtividadeInfantil : RepositorioBase<AtividadeInfantil>//, IRepositorioAviso
    {
        public RepositorioAtividadeInfantil(ISgpContext context) : base(context)
        {
        }

        public async Task<Aviso> ObterPorClassroomId(long avisoClassroomId)
        {
            var query = @"select * from atividade_infantil where aviso_classroom_id = @avisoClassroomId";
            return await database.Conexao.QueryFirstOrDefaultAsync<Aviso>(query, new { avisoClassroomId });
        }
        
        public async Task<IEnumerable<MuralAvisosRetornoDto>> ObterPorAulaId(long aulaId)
        {
            var query = @"select id, criado_em as DataPublicacao, mensagem, email from aviso where aula_id = @aulaId";
            return await database.Conexao.QueryAsync<MuralAvisosRetornoDto>(query, new { aulaId });
        }
    }
}
