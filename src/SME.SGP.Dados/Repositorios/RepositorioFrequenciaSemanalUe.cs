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

        public Task<IEnumerable<PainelEducacionalFrequenciaSemanalUeDto>> ObterFrequenciaSemanalUe(string codigoUe, int anoLetivo)
        {
            string query = @"select data_aula as DataAula,
                                 percentual_frequencia as PercentualFrequencia
                                 from painel_educacional_consolidacao_frequencia_semanal
                                 where 1 = 1 ";

            if (!string.IsNullOrWhiteSpace(codigoUe))
                query += " AND codigo_ue = @codigoUe ";

            if (anoLetivo > 0)
                query += " AND ano_letivo = @anoLetivo ";

            return database.Conexao.QueryAsync<PainelEducacionalFrequenciaSemanalUeDto>(query, new
            {
                codigoUe,
                anoLetivo
            });
        }
    }
}
