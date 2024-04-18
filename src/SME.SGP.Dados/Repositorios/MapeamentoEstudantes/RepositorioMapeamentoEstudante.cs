using Dapper;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioMapeamentoEstudante : RepositorioBase<MapeamentoEstudante>, IRepositorioMapeamentoEstudante
    {
        public const int SECAO_ETAPA_1 = 1;
        public const int SECAO_ORDEM_1 = 1;
        public const int FILTRO_TODOS = -99;

        public RepositorioMapeamentoEstudante(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        { }

        public async Task<MapeamentoEstudante> ObterMapeamentoEstudantePorId(long id)
        {
            const string query = @" select me.*, mes.*, qme.*, rme.*, sme.*, q.*, op.*
                            from mapeamento_estudante me
                            inner join mapeamento_estudante_secao mes on mes.mapeamento_estudante_id = me.id
                                and not mes.excluido
                            inner join secao_mapeamento_estudante sme on sme.id = mes.secao_mapeamento_estudante_id
                                and not sme.excluido
                            inner join mapeamento_estudante_questao qme on qme.mapeamento_estudante_secao_id = mes.id
                                and not qme.excluido
                            inner join questao q on q.id = qme.questao_id
                                and not q.excluido
                            inner join mapeamento_estudante_resposta rme on rme.questao_mapeamento_estudante_id = qme.id
                                and not rme.excluido
                            left join opcao_resposta op on op.id = rme.resposta_id
                                and not op.excluido
                            where me.id = @id
                            and not me.excluido;";

            var mapeamento = new MapeamentoEstudante();

            await database.Conexao
                .QueryAsync<MapeamentoEstudante, MapeamentoEstudanteSecao, QuestaoMapeamentoEstudante,
                    RespostaMapeamentoEstudante, SecaoMapeamentoEstudante, Questao, OpcaoResposta, MapeamentoEstudante>(
                    query, (mapeamentoEstudante, mapeamentoSecao, questaoMapeamentoEstudante, respostaMapeamentoEstudante,
                        secaoMapeamentoEstudante, questao, opcaoResposta) =>
                    {
                        if (mapeamento.Id == 0)
                            mapeamento = mapeamentoEstudante;

                        var secao = mapeamento.Secoes.FirstOrDefault(c => c.Id == mapeamentoSecao.Id);

                        if (secao.EhNulo())
                        {
                            mapeamentoSecao.SecaoMapeamentoEstudante = secaoMapeamentoEstudante;
                            secao = mapeamentoSecao;
                            mapeamento.Secoes.Add(secao);
                        }

                        var questaoRegistroAcao = secao.Questoes.FirstOrDefault(c => c.Id == questaoMapeamentoEstudante.Id);

                        if (questaoRegistroAcao.EhNulo())
                        {
                            questaoRegistroAcao = questaoMapeamentoEstudante;
                            questaoRegistroAcao.Questao = questao;
                            secao.Questoes.Add(questaoRegistroAcao);
                        }

                        var resposta = questaoRegistroAcao.Respostas.FirstOrDefault(c => c.Id == respostaMapeamentoEstudante.Id);

                        if (resposta.EhNulo())
                        {
                            resposta = respostaMapeamentoEstudante;
                            resposta.Resposta = opcaoResposta;
                            questaoRegistroAcao.Respostas.Add(resposta);
                        }

                        return mapeamento;
                    }, new { id });

            return mapeamento;
        }
    }
}
