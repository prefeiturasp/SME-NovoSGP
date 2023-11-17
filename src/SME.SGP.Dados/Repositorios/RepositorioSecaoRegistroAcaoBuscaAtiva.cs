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

        public async Task<IEnumerable<SecaoQuestionarioDto>> ObterSecoesQuestionarioDto(long? registroAcaoId = null)
        {
            var query = @"SELECT sraba.id
                                , sraba.nome
                                , sraba.questionario_id as questionarioId
                                , rabas.concluido
                                , sraba.etapa
                                , sraba.ordem
                                , sraba.nome_componente as nomeComponente
                         FROM secao_registro_acao_busca_ativa sraba 
                         inner join questionario q on q.id = sraba.questionario_id 
                         left join registro_acao_busca_ativa_secao rabas on rabas.registro_acao_busca_ativa_id = @registroAcaoId 
                                                                 and rabas.secao_registro_acao_id = sraba.id
                                                                 and not rabas.excluido   
                         WHERE not sraba.excluido 
                               AND q.tipo = @tipoQuestionario
                         ORDER BY sraba.etapa, sraba.ordem ";

            return await database.Conexao.QueryAsync<SecaoQuestionarioDto>(query, new { tipoQuestionario = (int)TipoQuestionario.RegistroAcaoBuscaAtiva, registroAcaoId = registroAcaoId ?? 0 });
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
