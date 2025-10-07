using Npgsql;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.SondagemEscrita;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSondagemEscritaUe : IRepositorioSondagemEscritaUe
    {
        private readonly string sondagemConnectionString;

        public RepositorioSondagemEscritaUe(IConfiguration configuration)
        {
            sondagemConnectionString = configuration.GetSection("SondagemConnectionString").Value
                ?? throw new InvalidOperationException("A connection string 'SondagemConnectionString' não foi encontrada nas secrets.");
        }

        public async Task<IEnumerable<SondagemEscritaUeDto>> ObterSondagemEscritaAsync()
        {
            string anoMinimo = "2019";

                string query = @"SELECT 
                                pp.""schoolCodeEol"" AS CodigoUe,
                                pp.""dreCodeEol"" AS CodigoDre,
                                pp.""yearClassroom"" AS SerieAno,
                                COUNT(CASE WHEN unpivoted.nivelEscrita = 'PS' THEN 1 END) AS PreSilabico,
                                COUNT(CASE WHEN unpivoted.nivelEscrita = 'SSV' THEN 1 END) AS SilabicoSemValor,
                                COUNT(CASE WHEN unpivoted.nivelEscrita = 'SCV' THEN 1 END) AS SilabicoComValor,
                                COUNT(CASE WHEN unpivoted.nivelEscrita = 'SA' THEN 1 END) AS SilabicoAlfabetico,
                                COUNT(CASE WHEN unpivoted.nivelEscrita = 'A' THEN 1 END) AS Alfabetico,
                                COUNT(CASE WHEN unpivoted.nivelEscrita IS NULL OR unpivoted.nivelEscrita = '' THEN 1 END) AS SemPreenchimento,
                                pp.""schoolYear"" AS AnoLetivo,
                                COUNT(pp.""studentCodeEol"") AS QuantidadeAluno,
                                unpivoted.periodo AS Bimestre
                            FROM 
                                ""PortuguesePolls"" pp
                            CROSS JOIN LATERAL (
                                VALUES
                                    (1, pp.""writing1B""),
                                    (2, pp.""writing2B""),
                                    (3, pp.""writing3B""),
                                    (4, pp.""writing4B"")
                            ) AS unpivoted(periodo, nivelEscrita)
                            WHERE 
                                pp.""schoolYear"" >= @anoMinimo
                            GROUP BY 
                                pp.""schoolCodeEol"", pp.""dreCodeEol"", pp.""yearClassroom"", pp.""schoolYear"", unpivoted.periodo
                            ORDER BY 
                                pp.""schoolCodeEol"" ASC, unpivoted.periodo ASC;";

                using var connection = new NpgsqlConnection(sondagemConnectionString);
                await connection.OpenAsync();
                return await connection.QueryAsync<SondagemEscritaUeDto>(query, new { anoMinimo });
        }
    }
}
