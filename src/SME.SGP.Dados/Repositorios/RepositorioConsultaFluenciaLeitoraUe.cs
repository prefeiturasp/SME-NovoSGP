using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoFluenciaLeitoraUe;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioConsultaFluenciaLeitoraUe : IRepositorioConsultaFluenciaLeitoraUe
    {
        private readonly ISgpContextConsultas database;
        public RepositorioConsultaFluenciaLeitoraUe(ISgpContextConsultas database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<ConsolidacaoFluenciaLeitoraUeDto>> ObterFluenciaLeitoraUe(FiltroPainelEducacionalFluenciaLeitoraUe filtro)
        {
            string query = @"SELECT 
                                    turma
                                  , alunos_previstos as alunosPrevistos
                                  , alunos_avaliados as alunosAvaliados                                  
                                  , pre_leitor_total AS preLeitorTotal
                                  , fluencia
                                  , quantidade_aluno_fluencia AS quantidadeAlunoFluencia
                                  , percentual_fluencia AS percentualFluencia
                           FROM painel_educacional_consolidacao_fluencia_leitora_ue
                           WHERE ano_letivo = @anoLetivo AND codigo_ue = @codigoUe AND tipo_avaliacao = @tipoAvaliacao";

            return await database.Conexao.QueryAsync<ConsolidacaoFluenciaLeitoraUeDto>(query, new
            {
                anoLetivo = filtro.AnoLetivo,
                tipoAvaliacao = filtro.TipoAvaliacao,
                codigoUe = filtro.CodigoUe
            });
        }
    }
}
