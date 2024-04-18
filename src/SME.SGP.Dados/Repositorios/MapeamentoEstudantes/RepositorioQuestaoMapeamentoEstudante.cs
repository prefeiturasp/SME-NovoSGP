using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.MapeamentoEstudantes;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioQuestaoMapeamentoEstudante : RepositorioBase<QuestaoMapeamentoEstudante>, IRepositorioQuestaoMapeamentoEstudante
    {
        public RepositorioQuestaoMapeamentoEstudante(ISgpContext repositorio, IServicoAuditoria servicoAuditoria) : base(repositorio, servicoAuditoria)
        { }

        public async Task<IEnumerable<long>> ObterQuestoesPorSecaoId(long mapeamentoEstudanteSecaoId)
        {
            var query = "select id from mapeamento_estudante_questao where mapeamento_estudante_secao_id = @mapeamentoEstudanteSecaoId";
            return await database.Conexao.QueryAsync<long>(query, new { mapeamentoEstudanteSecaoId });
        }

        public async Task<IEnumerable<RespostaQuestaoMapeamentoEstudanteDto>> ObterRespostasMapeamentoEstudante(long mapeamentoEstudanteId)
        {
            var query = @$"select mer.Id
                            , meq.questao_id as QuestaoId
                            , mer.resposta_id as RespostaId
                            , mer.texto 
                            , a.*
                          from mapeamento_estudante_secao mes  
                         inner join mapeamento_estudante_questao meq on meq.mapeamento_estudante_secao_id = mes.id
                         inner join mapeamento_estudante_resposta mer on mer.questao_mapeamento_estudante_id = meq.id
                          left join arquivo a on a.id = mer.arquivo_id 
                         where not mes.excluido 
                           and not meq.excluido 
                           and not mer.excluido 
                           and mes.mapeamento_estudante_id = @mapeamentoEstudanteId";

            return await database.Conexao.QueryAsync<RespostaQuestaoMapeamentoEstudanteDto, Arquivo, RespostaQuestaoMapeamentoEstudanteDto>(query,
                (resposta, arquivo) =>
                {
                    resposta.Arquivo = arquivo;
                    return resposta;
                }, new { mapeamentoEstudanteId });
        }

    }
}
