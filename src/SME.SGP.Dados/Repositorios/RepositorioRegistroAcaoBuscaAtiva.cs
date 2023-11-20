using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRegistroAcaoBuscaAtiva : RepositorioBase<RegistroAcaoBuscaAtiva>, IRepositorioRegistroAcaoBuscaAtiva
    {
        public RepositorioRegistroAcaoBuscaAtiva(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<string>> ObterCodigoArquivoPorRegistroAcaoId(long id)
        {
            var sql = @"select
                            a.codigo
                        from
                            registro_acao_busca_ativa raba
                        inner join registro_acao_busca_ativa_secao rabas on
                            raba.id = rabas.registro_acao_busca_ativa_id
                        inner join registro_acao_busca_ativa_questao qraba on
                            rabas.id = qraba.registro_acao_busca_ativa_secao_id
                        inner join registro_acao_busca_ativa_resposta rraba on
                            qraba.id = rraba.questao_registro_acao_id
                        inner join arquivo a on
                            rraba.arquivo_id = a.id
                        where
                            raba.id = @id";

            return await database.Conexao.QueryAsync<string>(sql.ToString(), new { id });
        }

        public Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoCabecalhoPorId(long id)
        {
            throw new System.NotImplementedException();
        }

        public Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoComTurmaPorId(long id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoPorId(long id)
        {
            const string query = @" select raba.*, rabas.*, qraba.*, rraba.*, sraba.*, q.*, op.*
                                    from registro_acao_busca_ativa raba
                                    inner join registro_acao_busca_ativa_secao rabas on rabas.registro_acao_busca_ativa_id = raba.id
	                                    and not rabas.excluido
                                    inner join secao_registro_acao_busca_ativa sraba on sraba.id = rabas.secao_registro_acao_id
	                                    and not sraba.excluido
                                    inner join registro_acao_busca_ativa_questao qraba on qraba.registro_acao_busca_ativa_secao_id = rabas.id
	                                    and not qraba.excluido
                                    inner join questao q on q.id = qraba.questao_id
	                                    and not q.excluido
                                    inner join registro_acao_busca_ativa_resposta rraba on rraba.questao_registro_acao_id = qraba.id
	                                    and not rraba.excluido
                                    left join opcao_resposta op on op.id = rraba.resposta_id
	                                    and not op.excluido
                                    where raba.id = @id
                                    and not raba.excluido;";

            var registroAcao = new RegistroAcaoBuscaAtiva();

            await database.Conexao
                .QueryAsync<RegistroAcaoBuscaAtiva, RegistroAcaoBuscaAtivaSecao, QuestaoRegistroAcaoBuscaAtiva,
                    RespostaRegistroAcaoBuscaAtiva, SecaoRegistroAcaoBuscaAtiva, Questao, OpcaoResposta, RegistroAcaoBuscaAtiva>(
                    query, (RegistroAcaoBuscaAtiva, registroAcaoSecao, questaoRegistroAcaoBuscaAtiva, respostaRegistroAcao,
                        secaoRegistroAcao, questao, opcaoResposta) =>
                    {
                        if (registroAcao.Id == 0)
                            registroAcao = RegistroAcaoBuscaAtiva;

                        var secao = registroAcao.Secoes.FirstOrDefault(c => c.Id == registroAcaoSecao.Id);

                        if (secao.EhNulo())
                        {
                            registroAcaoSecao.SecaoRegistroAcaoBuscaAtiva = secaoRegistroAcao;
                            secao = registroAcaoSecao;
                            registroAcao.Secoes.Add(secao);
                        }

                        var questaoRegistroAcao = secao.Questoes.FirstOrDefault(c => c.Id == questaoRegistroAcaoBuscaAtiva.Id);

                        if (questaoRegistroAcao.EhNulo())
                        {
                            questaoRegistroAcao = questaoRegistroAcaoBuscaAtiva;
                            questaoRegistroAcao.Questao = questao;
                            secao.Questoes.Add(questaoRegistroAcao);
                        }

                        var resposta = questaoRegistroAcao.Respostas.FirstOrDefault(c => c.Id == respostaRegistroAcao.Id);

                        if (resposta.EhNulo())
                        {
                            resposta = respostaRegistroAcao;
                            resposta.Resposta = opcaoResposta;
                            questaoRegistroAcao.Respostas.Add(resposta);
                        }

                        return registroAcao;
                    }, new { id });

            return registroAcao;
        }

        public Task<RegistroAcaoBuscaAtiva> ObterRegistroAcaoPorIdESecao(long id, long secaoId)
        {
            throw new System.NotImplementedException();
        }
    }
}
