using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.Frequencia;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalConsolidacaoFrequenciaDiariaConsulta : IRepositorioPainelEducacionalConsolidacaoFrequenciaDiariaConsulta
    {
        private readonly ISgpContextConsultas sgpContext;

        public RepositorioPainelEducacionalConsolidacaoFrequenciaDiariaConsulta(ISgpContextConsultas sgpContext)
        {
            this.sgpContext = sgpContext;
        }

        public async Task<IEnumerable<DadosParaConsolidarFrequenciaSemanalAlunoDto>> ObterFrequenciaSemanal(IEnumerable<DateTime> dataAulas)
        {
            var sql = @"select 
                            codigo_dre as CodigoDre,
                            codigo_ue as CodigoUe,
                            total_estudantes as TotalEstudantes,
                            total_presentes as TotalPresentes,  
                            percentual_frequencia as PercentualFrequencia,
                            data_aula as DataAula,
                            ano_letivo as AnoLetivo
                         from painel_educacional_consolidacao_frequencia_diaria_ue
                         where data_aula = any(@dataAulas)";
            var parametros = new { dataAulas };

            return await sgpContext.QueryAsync<DadosParaConsolidarFrequenciaSemanalAlunoDto>(sql, parametros);
        }
    }
}
