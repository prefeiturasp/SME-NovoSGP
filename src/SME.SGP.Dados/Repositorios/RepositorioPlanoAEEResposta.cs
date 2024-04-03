using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEEResposta : RepositorioBase<PlanoAEEResposta>, IRepositorioPlanoAEEResposta
    {
        public RepositorioPlanoAEEResposta(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<RespostaQuestaoDto>> ObterRespostasPorVersaoPlano(long versaoPlanoId)
        {
            var query = @"select par.*, a.*, paq.*
                          from plano_aee_questao paq 
                         inner join plano_aee_resposta par on par.plano_questao_id = paq.id
                          left join arquivo a on a.id = par.arquivo_id 
                         where paq.plano_aee_versao_id = @versaoPlanoId";

            return await database.Conexao.QueryAsync<PlanoAEEResposta, Arquivo, PlanoAEEQuestao, RespostaQuestaoDto>(query,
                (planoAEEResposta, arquivo, planoAEEQuestao) =>
                {
                    return new RespostaQuestaoDto()
                    {
                        Id = planoAEEResposta.Id,
                        QuestaoId = planoAEEQuestao.QuestaoId,
                        OpcaoRespostaId = planoAEEResposta.RespostaId,
                        Texto = planoAEEResposta.Texto,
                        PeriodoInicio = planoAEEResposta.PeriodoInicio,
                        PeriodoFim = planoAEEResposta.PeriodoFim,
                        Arquivo = arquivo
                    };
                }, new { versaoPlanoId });
        }
        public async Task Atualizar(string resposta, long id)
        {
            var sql = @"UPDATE plano_aee_resposta SET texto = @texto WHERE id = @id";

            await database.Conexao.QueryAsync<string>(sql.ToString(), new { texto = resposta, id });
        }
    }
}
