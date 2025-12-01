using Dapper;
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
    public class RepositorioNovoEncaminhamentoNAAPASecao : RepositorioBase<EncaminhamentoNAAPASecao>, IRepositorioNovoEncaminhamentoNAAPASecao
    {
        private const int ORDEM_SECAO_ITINERARIO = 3;
        public RepositorioNovoEncaminhamentoNAAPASecao(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }

        public async Task<AuditoriaDto> ObterAuditoriaNovoEncaminhamentoNaapaSecao(long id)
        {
            var query = "select * from encaminhamento_naapa_secao eas where not excluido and id = @id";

            return await database.Conexao.QueryFirstOrDefaultAsync<AuditoriaDto>(query, new { id });
        }

        public async Task<IEnumerable<long>> ObterIdsSecoesPorNovoEncaminhamentoNAAPAId(long encaminhamentoNAAPAId)
        {
            var query = "select id from encaminhamento_naapa_secao eas where not excluido and encaminhamento_naapa_id = @encaminhamentoNAAPAId";

            return await database.Conexao.QueryAsync<long>(query, new { encaminhamentoNAAPAId });
        }

        public async Task<bool> ExisteSecoesDeItineracia(long encaminhamentoNAAPAId)
        {
            var query = @$"select count(1) from secao_encaminhamento_naapa sen 
                          inner join encaminhamento_naapa_secao ens on ens.secao_encaminhamento_id = sen.id
                          where sen.ordem = @ordem and ens.encaminhamento_naapa_id = @encaminhamentoNAAPAId and not ens.excluido";

            return await database.Conexao.ExecuteScalarAsync<bool>(query, new { ordem = ORDEM_SECAO_ITINERARIO, encaminhamentoNAAPAId });
        }
    }
}