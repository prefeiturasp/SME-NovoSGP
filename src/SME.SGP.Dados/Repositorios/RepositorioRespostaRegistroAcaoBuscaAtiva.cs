using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRespostaRegistroAcaoBuscaAtiva : RepositorioBase<RespostaRegistroAcaoBuscaAtiva>, IRepositorioRespostaRegistroAcaoBuscaAtiva
    {
        public RepositorioRespostaRegistroAcaoBuscaAtiva(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {}
        public async Task<IEnumerable<RespostaRegistroAcaoBuscaAtiva>> ObterPorQuestaoRegistroAcaoId(long questaoRegistroAcaoId)
        {
            var query = @"select 
                                id, 
                                questao_registro_acao_id AS QuestaoRegistroAcaoBuscaAtivaId, 
                                resposta_id AS RespostaId, 
                                arquivo_id AS ArquivoId, 
                                texto AS Texto, 
                                excluido, 
                                criado_em, 
                                criado_por, 
                                alterado_em, 
                                alterado_por, 
                                criado_rf, 
                                alterado_rf 
                            from registro_acao_busca_ativa_resposta 
                            where not excluido and questao_registro_acao_id = @questaoRegistroAcaoId";
            return await database.Conexao.QueryAsync<RespostaRegistroAcaoBuscaAtiva>(query, new { questaoRegistroAcaoId });
        }
    }
}
