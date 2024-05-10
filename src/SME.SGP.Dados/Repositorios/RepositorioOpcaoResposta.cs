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

        public Task<IEnumerable<OpcaoRespostaSimplesDto>> ObterOpcoesRespostaPorNomeComponenteQuestaoTipoQuestionario(string nomeComponente, TipoQuestionario tipoQuestionario)
        {
            var query = @"select opcres.id, opcres.ordem, opcres.nome, opcres.questao_id as questaoId, opcres.observacao
                          from opcao_resposta opcres
                          inner join questao q on q.id = opcres.questao_id 
                          inner join questionario q2 on q2.id = q.questionario_id 
                          where q2.tipo = @tipoQuestionario and not opcres.excluido and not q.excluido 
                                and q.nome_componente = @nomeComponente
                          order by opcres.ordem";

            return database.Conexao.QueryAsync<OpcaoRespostaSimplesDto>(query, new { nomeComponente, tipoQuestionario = (int)tipoQuestionario });
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
