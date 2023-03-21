using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioItineranciaQuestao : RepositorioBase<ItineranciaQuestao>, IRepositorioItineranciaQuestao
    {
        public RepositorioItineranciaQuestao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }


        public async Task ExcluirItineranciaQuestao(long questaoId, long itineranciaId)
        {
            await database.Conexao.ExecuteScalarAsync(@"delete from itinerancia_questao iq where itinerancia_id = @itineranciaId and id = @questaoId", new { questaoId, itineranciaId });
        }

        public async Task<bool> ExcluirItineranciaQuestaoPorArquivo(long arquivoId)
        {
            return await database.Conexao.ExecuteAsync("delete from itinerancia_questao where  arquivo_id  = @arquivoId", new {arquivoId}) > 0;
        }
    }
}
