using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioPeriodicoPAPResposta : RepositorioBase<RelatorioPeriodicoPAPResposta>, IRepositorioRelatorioPeriodicoPAPResposta
    {
        public RepositorioRelatorioPeriodicoPAPResposta(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<RelatorioPeriodicoPAPResposta>> ObterRespostas(long papSecaoId)
        {
            var query = @"select rppr.*, rppq.*, a.* 
                        from relatorio_periodico_pap_questao rppq
                        inner join relatorio_periodico_pap_resposta rppr on rppr.relatorio_periodico_pap_questao_id = rppq.id
                        left join arquivo a on a.id = rppr.arquivo_id 
                        where rppq.relatorio_periodico_pap_secao_id = @papSecaoId
                           and not rppq.excluido 
                           and not rppr.excluido";

            return await database.Conexao.QueryAsync<RelatorioPeriodicoPAPResposta, RelatorioPeriodicoPAPQuestao, Arquivo, RelatorioPeriodicoPAPResposta>(query,
            (resposta, periodicoPAPQuestao, arquivo) =>
            {
                resposta.RelatorioPeriodicoQuestao = periodicoPAPQuestao;
                resposta.Arquivo = arquivo;

                return resposta;
            }, new { papSecaoId });
        }
    }
}
