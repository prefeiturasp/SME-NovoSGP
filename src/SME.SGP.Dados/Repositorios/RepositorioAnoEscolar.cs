using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioAnoEscolar : IRepositorioAnoEscolar
    {
        private readonly ISgpContext database;

        public RepositorioAnoEscolar(ISgpContext database)
        {
            this.database = database ?? throw new ArgumentNullException(nameof(database));
        }
        public async Task<IEnumerable<ModalidadeAnoDto>> ObterPorModalidadeCicloId(Modalidade modalidade, long cicloId)
        {
            var query = new StringBuilder(@"select modalidade, ano from tipo_ciclo_ano tca 
                        where  tca.modalidade = @modalidadeId ");

            if (cicloId > 0)
                query.AppendLine("and tca.tipo_ciclo_id = @cicloId");

            return await database.Conexao.QueryAsync<ModalidadeAnoDto>(query.ToString(), new { cicloId, modalidadeId = (int)modalidade });

        }
    }
}