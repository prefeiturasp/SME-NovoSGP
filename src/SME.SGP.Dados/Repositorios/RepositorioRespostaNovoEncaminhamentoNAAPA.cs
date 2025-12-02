using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRespostaNovoEncaminhamentoNAAPA : RepositorioBase<RespostaEncaminhamentoNAAPA>, IRepositorioRespostaNovoEncaminhamentoNAAPA
    {
        public RepositorioRespostaNovoEncaminhamentoNAAPA(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<RespostaEncaminhamentoNAAPA>> ObterPorQuestaoEncaminhamentoId(long questaoEncaminhamentoNAAPAId)
        {
            var query = "select * from encaminhamento_naapa_resposta where not excluido and questao_encaminhamento_id = @questaoEncaminhamentoNAAPAId";

            return await database.Conexao.QueryAsync<RespostaEncaminhamentoNAAPA>(query, new { questaoEncaminhamentoNAAPAId });
        }

        public async Task<bool> RemoverPorArquivoId(long arquivoId)
        {
            await Task.CompletedTask;

            var sql =
                $@"
                    update encaminhamento_naapa_resposta 
                        set excluido = true,
                            arquivo_id = null
                    where arquivo_id = @arquivoId 
                ";

            return (
                await database
                .Conexao
                .ExecuteAsync(sql, new { arquivoId })
                ) > 0;
        }

        public async Task<IEnumerable<long>> ObterArquivosPorQuestaoId(long questaoEncaminhamentoNAAPAId)
        {
            var query = "select arquivo_id from encaminhamento_naapa_resposta where questao_encaminhamento_id = @questaoEncaminhamentoNAAPAId and arquivo_id is not null";

            return await database.Conexao.QueryAsync<long>(query, new { questaoEncaminhamentoNAAPAId });
        }

        public async Task<IEnumerable<string>> ObterNomesComponenteSecaoComAnexosEmPdf(long encaminhamentoId)
        {
            var query = @"select distinct nome_componente
                        from encaminhamento_naapa ea
                        inner join encaminhamento_naapa_secao eas on ea.id = eas.encaminhamento_naapa_id
                        inner join secao_encaminhamento_naapa sen on sen.id = eas.secao_encaminhamento_id
                        inner join encaminhamento_naapa_questao qea on eas.id = qea.encaminhamento_naapa_secao_id
                        inner join encaminhamento_naapa_resposta rea on qea.id = rea.questao_encaminhamento_id
                        inner join arquivo a on rea.arquivo_id = a.id
                        where ea.id = @encaminhamentoId 
                          and not rea.excluido
                          and a.tipo_conteudo like '%pdf'";

            return await database.Conexao.QueryAsync<string>(query, new { encaminhamentoId });
        }
    }
}