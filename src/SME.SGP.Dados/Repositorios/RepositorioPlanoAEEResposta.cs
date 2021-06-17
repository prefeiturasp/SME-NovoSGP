using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPlanoAEEResposta : RepositorioBase<PlanoAEEResposta>, IRepositorioPlanoAEEResposta
    {
        public RepositorioPlanoAEEResposta(ISgpContext database) : base(database)
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
    }
}
