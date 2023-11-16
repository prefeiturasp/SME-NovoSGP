using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSecaoRegistroAcaoBuscaAtiva : RepositorioBase<SecaoRegistroAcaoBuscaAtiva>, IRepositorioSecaoRegistroAcaoBuscaAtiva
    {

        public RepositorioSecaoRegistroAcaoBuscaAtiva(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public Task<SecaoQuestionarioDto> ObterSecaoQuestionarioDtoPorId(long secaoId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<SecaoQuestionarioDto>> ObterSecoesQuestionarioDto(long? registroAcaoId = null)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<SecaoRegistroAcaoBuscaAtiva>> ObterSecoesRegistroAcaoBuscaAtiva(long? registroAcaoId = null)
        {
            var query = new StringBuilder(@"SELECT sraba.*, rabas.*, q.*
                                            FROM secao_registro_acao_busca_ativa sraba 
                                                join questionario q on q.id = sraba.questionario_id 
                                                left join registro_acao_busca_ativa_secao rabas on rabas.registro_acao_busca_ativa_id = @registroAcaoId
                                                                                        and rabas.secao_registro_acao_id = sraba.id 
                                                                                        and not rabas.excluido  
                                            WHERE not sraba.excluido 
                                            ORDER BY sraba.etapa, sraba.ordem; ");

            return await database.Conexao
                .QueryAsync<SecaoRegistroAcaoBuscaAtiva, RegistroAcaoBuscaAtivaSecao, Questionario, SecaoRegistroAcaoBuscaAtiva>(
                    query.ToString(), (secaoRegistroAcao, registroAcaoSecao, questionario) =>
                    {
                        secaoRegistroAcao.RegistroBuscaAtivaSecao = registroAcaoSecao;
                        secaoRegistroAcao.Questionario = questionario;
                        return secaoRegistroAcao;
                    }, new { registroAcaoId = registroAcaoId ?? 0 }, splitOn: "id");
        }
    }
}
