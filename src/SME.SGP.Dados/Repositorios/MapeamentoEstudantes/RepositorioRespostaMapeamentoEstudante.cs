using Elastic.Apm.Api;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRespostaMapeamentoEstudante : RepositorioBase<RespostaMapeamentoEstudante>, IRepositorioRespostaMapeamentoEstudante
    {
        public RepositorioRespostaMapeamentoEstudante(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {}

        public async Task<IEnumerable<RespostaMapeamentoEstudante>> ObterPorQuestaoMapeamentoEstudanteId(long questaoMapeamentoEstudanteId)
        {
            var query = "select * from mapeamento_estudante_resposta where not excluido and questao_mapeamento_estudante_id = @questaoMapeamentoEstudanteId";
            return await database.Conexao.QueryAsync<RespostaMapeamentoEstudante>(query, new { questaoMapeamentoEstudanteId });
        }

        public Task<long> ObterIdOpcaoRespostaPorNomeComponenteQuestao(string nomeComponenteQuestao, string descricaoOpcaoResposta, int? ordemOpcaoResposta = null)
        => database.Conexao.QueryFirstOrDefaultAsync<long>(@$"select or2.id from opcao_resposta or2 
                                                              inner join questao q on q.id = or2.questao_id 
                                                              inner join questionario q2 on q2.id = q.questionario_id 
                                                              where not or2.excluido and not q.excluido and not q2.excluido 
                                                                    and q2.tipo = @tipoQuestionario
                                                                    and q.nome_componente = @nomeComponente
                                                                    {(!string.IsNullOrEmpty(descricaoOpcaoResposta) ? "and or2.nome = @descricaoOpcaoResposta" : string.Empty)}
                                                                    {(ordemOpcaoResposta.HasValue ? "and or2.ordem = @ordemOpcaoResposta" : string.Empty)}",
                                                           new { nomeComponenteQuestao, 
                                                                 descricaoOpcaoResposta, 
                                                                 ordemOpcaoResposta });
    }
}
