using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioOpcaoResposta : RepositorioBase<OpcaoResposta>, IRepositorioOpcaoResposta
    {
        public RepositorioOpcaoResposta(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public Task<IEnumerable<OpcaoRespostaSimplesDto>> ObterOpcoesRespostaPorQuestaoId(long QuestaoId)
        {
            var query = @"select or2.id, or2.ordem, or2.nome, or2.questao_id as questaoId, or2.observacao
                          from opcao_resposta or2
                          where or2.questao_id = @QuestaoId;";
           
            return database.Conexao.QueryAsync<OpcaoRespostaSimplesDto>(query, new { QuestaoId });
        }
    }
}
