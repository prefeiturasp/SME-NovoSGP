using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaSemanalUe;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFrequenciaSemanalUe : IRepositorioFrequenciaSemanalUe
    {
        private readonly ISgpContext database;
        public RepositorioFrequenciaSemanalUe(ISgpContext database)
        {
            this.database = database;
        }

        public Task<IEnumerable<PainelEducacionalFrequenciaSemanalUeDto>> ObterFrequenciaSemanalUe(string codigoDre, int anoLetivo)
        {
            string query = @"select data_semanal as Data,
                                 percentual_frequencia as PercentualFrequencia,
                                 from painel_educacional_consolidacao_frequencia_semanal_ue
                                 where 1 = 1 ";

            if (!string.IsNullOrWhiteSpace(codigoDre))
                query += " AND codigo_dre = @codigoDre ";

            if (anoLetivo > 0)
                query += " AND ano_letivo = @anoLetivo ";          

            return database.Conexao.QueryAsync<PainelEducacionalFrequenciaSemanalUeDto>(query, new
            {
                codigoDre,
                anoLetivo
            });
        }
    }
}
