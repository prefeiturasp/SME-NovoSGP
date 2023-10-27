using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioRelatorioPeriodicoPAPQuestao : RepositorioBase<RelatorioPeriodicoPAPQuestao>, IRepositorioRelatorioPeriodicoPAPQuestao
    {
        public RepositorioRelatorioPeriodicoPAPQuestao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<IEnumerable<Questao>> ObterQuestoesMigracao()
        {
            var query = @"  select q2.*
                            from secao_relatorio_periodico_pap srpp
                            join questionario q on q.id = srpp.questionario_id
                            join questao q2 on q2.questionario_id = q.id
                            where srpp.nome_componente in ('SECAO_AVANC_APREND_BIMES', 'SECAO_OBS', 'SECAO_DIFIC_APRES')";

            return await database.Conexao.QueryAsync<Questao>(query, new { });
        }

    }
}
