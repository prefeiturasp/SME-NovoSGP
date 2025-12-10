using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestaoNovoEncaminhamentoNAAPA : RepositorioBase<QuestaoEncaminhamentoNAAPA>, IRepositorioQuestaoNovoEncaminhamentoNAAPA
    {
        private const int ETAPA_1 = 1;
        private const string QUESTAO_PRIORIDADE = "Prioridade";

        public RepositorioQuestaoNovoEncaminhamentoNAAPA(ISgpContext repositorio, IServicoAuditoria servicoAuditoria) : base(repositorio, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<PrioridadeNovoEncaminhamentoNAAPADto>> ObterPrioridadeEncaminhamento()
        {
            var query = @"select opre.id, opre.nome
                            from secao_encaminhamento_naapa sen
                            inner join questionario qti on qti.id = sen.questionario_id
                            inner join questao qta on qta.questionario_id = qti.id
                            inner join opcao_resposta opre on opre.questao_id = qta.id
                            where sen.etapa = @etapa and qta.nome = @questaoPrioridade and qti.tipo = @tipoQuestionario
                            order by opre.ordem";

            return await database.Conexao.QueryAsync<PrioridadeNovoEncaminhamentoNAAPADto>(query, new { etapa = ETAPA_1, questaoPrioridade = QUESTAO_PRIORIDADE, tipoQuestionario = (int)TipoQuestionario.EncaminhamentoNAAPA });
        }

        public async Task<QuestaoEncaminhamentoNAAPA> ObterQuestaoEnderecoResidencialPorEncaminhamentoId(long encaminhamentoNAAPAId)
        {
            var query = @"select enq.id as QuestaoId, enr.*
                            from encaminhamento_naapa_questao enq                           
                            inner join encaminhamento_naapa_secao ens on ens.id = enq.encaminhamento_naapa_secao_id 
                            inner join secao_encaminhamento_naapa sen on sen.id = ens.secao_encaminhamento_id 
                            inner join questao q on q.id = enq.questao_id 
                            left join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id and not enr.excluido 
                            where not enq.excluido and not ens.excluido 
                                and sen.nome_componente = 'INFORMACOES_ESTUDANTE' 
                                and q.nome_componente = 'ENDERECO_RESIDENCIAL'
                                and ens.encaminhamento_naapa_id  = @encaminhamentoNAAPAId";

            QuestaoEncaminhamentoNAAPA retorno = null;
            await database.Conexao.QueryAsync<QuestaoEncaminhamentoNAAPA, RespostaEncaminhamentoNAAPA, QuestaoEncaminhamentoNAAPA>(query,
                                        (questaoNAAPA, respostaNAAPA) =>
                                        {
                                            if (retorno.EhNulo()) retorno = questaoNAAPA;
                                            if (respostaNAAPA.NaoEhNulo()) retorno.Respostas.Add(respostaNAAPA);
                                            return retorno;

                                        },
                                        new { encaminhamentoNAAPAId });
            return retorno;

        }

        public async Task<QuestaoEncaminhamentoNAAPA> ObterQuestaoTurmasProgramaPorEncaminhamentoId(long encaminhamentoNAAPAId)
        {
            var query = @"select enq.id as QuestaoId, enr.*
                            from encaminhamento_naapa_questao enq                           
                            inner join encaminhamento_naapa_secao ens on ens.id = enq.encaminhamento_naapa_secao_id 
                            inner join secao_encaminhamento_naapa sen on sen.id = ens.secao_encaminhamento_id 
                            inner join questao q on q.id = enq.questao_id 
                            left join encaminhamento_naapa_resposta enr on enr.questao_encaminhamento_id = enq.id and not enr.excluido 
                            where not enq.excluido and not ens.excluido 
                                and sen.nome_componente = 'INFORMACOES_ESTUDANTE' 
                                and q.nome_componente = 'TURMAS_PROGRAMA'
                                and ens.encaminhamento_naapa_id  = @encaminhamentoNAAPAId";

            QuestaoEncaminhamentoNAAPA retorno = null;
            await database.Conexao.QueryAsync<QuestaoEncaminhamentoNAAPA, RespostaEncaminhamentoNAAPA, QuestaoEncaminhamentoNAAPA>(query,
                                        (questaoNAAPA, respostaNAAPA) =>
                                        {
                                            if (retorno.EhNulo()) retorno = questaoNAAPA;
                                            if (respostaNAAPA.NaoEhNulo()) retorno.Respostas.Add(respostaNAAPA);
                                            return retorno;

                                        },
                                        new { encaminhamentoNAAPAId });
            return retorno;
        }

        public async Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long encaminhamentoNAAPASecaoId)
        {
            var query = "select id from encaminhamento_naapa_questao qea where encaminhamento_naapa_secao_id = @encaminhamentoNAAPASecaoId";

            return await database.Conexao.QueryAsync<long>(query, new { encaminhamentoNAAPASecaoId });
        }

        public async Task<IEnumerable<RespostaQuestaoNovoEncaminhamentoNAAPADto>> ObterRespostasEncaminhamento(long encaminhamentoId)
        {
            return await ObterRespostasEncaminhamento(encaminhamentoId, "ens.encaminhamento_escolar_id = @encaminhamentoId");
        }

        public async Task<IEnumerable<RespostaQuestaoNovoEncaminhamentoNAAPADto>> ObterRespostasItinerarioEncaminhamento(long encaminhamentoSecaoId)
        {
            return await ObterRespostasEncaminhamento(encaminhamentoSecaoId, "ens.id = @encaminhamentoId");
        }

        private async Task<IEnumerable<RespostaQuestaoNovoEncaminhamentoNAAPADto>> ObterRespostasEncaminhamento(long encaminhamentoId, string condicao)
        {
            var query = @$"select ren.Id
                            , qen.questao_id as QuestaoId
                            , ren.resposta_id as RespostaId
                            , ren.texto 
                            , a.*
                          from encaminhamento_naapa_secao ens 
                         inner join encaminhamento_escolar_questao qen on qen.encaminhamento_escolar_secao_id = ens.id
                         inner join encaminhamento_escolar_resposta ren on ren.questao_encaminhamento_id = qen.id
                          left join arquivo a on a.id = ren.arquivo_id 
                         where not ens.excluido 
                           and not qen.excluido 
                           and not ren.excluido 
                           and {condicao}";

            return await database.Conexao.QueryAsync<RespostaQuestaoNovoEncaminhamentoNAAPADto, Arquivo, RespostaQuestaoNovoEncaminhamentoNAAPADto>(query,
                (resposta, arquivo) =>
                {
                    resposta.Arquivo = arquivo;
                    return resposta;
                }, new { encaminhamentoId });
        }
    }
}