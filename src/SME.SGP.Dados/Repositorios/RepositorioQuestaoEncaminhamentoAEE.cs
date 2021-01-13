using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestaoEncaminhamentoAEE : RepositorioBase<QuestaoEncaminhamentoAEE>, IRepositorioQuestaoEncaminhamentoAEE
    {
        public RepositorioQuestaoEncaminhamentoAEE(ISgpContext repositorio) : base(repositorio)
        {
        }

        public async Task<IEnumerable<QuestaoRespostaAeeDto>> ObterListaPorQuestionario(long questionarioId)
        {
            return await ObterListaPorQuestionarioEncaminhamento(questionarioId, null);
        }

        public async Task<IEnumerable<QuestaoRespostaAeeDto>> ObterListaPorQuestionarioEncaminhamento(long questionarioId, long? encaminhamentoId)
        {
			var joinRespostas = 
				encaminhamentoId.HasValue ? "" : "and false";
			var whereRespostas = 
				encaminhamentoId.HasValue ? "and eas.encaminhamento_aee_id = @encaminhamentoId" : "";

			var sql = 
                $@"
				select 
					q.id QuestaoId,
					q.ordem QuestaoOrder,
					q.nome QuestaoNome,
					q.observacao QuestaoObservacao,
					q.obrigatorio QuestaoObrigatorio,
					q.tipo QuestaoTipo,
					q.opcionais QuestaoOpcionais,
					opr.id OpcaoRespostaId,
					opr.questao_complementar_id QuestaoComplementarId,
					opr.ordem OpcaoRespostaOrdem,
					opr.nome OpcaoRespostaNome,
					rea.id RespostaEncaminhamentoId,
					rea.resposta_id RespostaEncaminhamentoOpcaoRespostaId,
					rea.texto RespostaTexto,
					rea.arquivo_id RespostaArquivoId,
					arq.nome ArquivoNome,
					arq.tipo ArquivoTipo,
					arq.codigo ArquivoCodigo,
					arq.tipo_conteudo ArquivoTipoConteudo
				from questao q
				left join opcao_resposta opr on opr.questao_id = q.id and not opr.excluido 
				left join questao_encaminhamento_aee qea on qea.questao_id = q.id and not qea.excluido {joinRespostas}
				left join resposta_encaminhamento_aee rea on rea.questao_encaminhamento_id = qea.id and not rea.excluido {joinRespostas}
				left join encaminhamento_aee_secao eas on eas.id = qea.encaminhamento_aee_secao_id and not eas.excluido {joinRespostas}
				left join arquivo arq on arq.id = rea.arquivo_id {joinRespostas}
				where not q.excluido 
				and q.questionario_id = @questionarioId
				{whereRespostas}
                ";
			return await database
				.Conexao
				.QueryAsync<QuestaoRespostaAeeDto>(sql, new { questionarioId, encaminhamentoId });
        }

        public async Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long encaminhamentoAEESecaoId)
        {
            var query = "select id from questao_encaminhamento_aee qea where encaminhamento_aee_secao_id = @encaminhamentoAEESecaoId";

            return await database.Conexao.QueryAsync<long>(query, new { encaminhamentoAEESecaoId });
        }
    }
}
