using Dapper;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaDiaria;
using SME.SGP.Infra.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalRegistroFrequenciaDiariaConsulta : IRepositorioPainelEducacionalRegistroFrequenciaDiariaConsulta
    {
        private readonly ISgpContextConsultas contextoConsulta;

        public RepositorioPainelEducacionalRegistroFrequenciaDiariaConsulta(ISgpContextConsultas contextoConsulta)
        {
            this.contextoConsulta = contextoConsulta ?? throw new ArgumentNullException(nameof(contextoConsulta));
        }

        public async Task<PaginacaoResultadoDto<RegistroFrequenciaDiariaUeDto>> ObterFrequenciaDiariaPorDre(FiltroFrequenciaDiariaDreDto filtro)
        {
            var paginacao = filtro.ObterPaginacao();
            DateTime.TryParse(filtro.DataFrequencia, out var dataFrequencia);

            var query = new StringBuilder();

            query.AppendLine("SELECT");
            query.AppendLine("   data_aula AS Data");
            query.AppendLine(" , ue");
            query.AppendLine(" , total_alunos AS QuantidadeAlunos");
            query.AppendLine(" , total_presencas AS QuantidadeAlunosPresentes");
            query.AppendLine(" , percentual AS PercentualFrequencia");
            query.AppendLine(" , nivel_frequencia AS NivelFrequencia");
            query.AppendLine("FROM painel_educacional_consolidacao_frequencia_diaria_teste");
            query.AppendLine("WHERE codigo_dre = @codigoDre AND ano_letivo = @anoLetivo");
            
            if (!string.IsNullOrEmpty(filtro.DataFrequencia))
            {
                query.AppendLine(" AND DATE(data_aula) = @dataFrequencia");
            }

            if (paginacao.QuantidadeRegistros > 0)
            {
                query.AppendFormat("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros);
            }

            query.AppendLine("; SELECT COUNT(id) FROM painel_educacional_consolidacao_frequencia_diaria_teste WHERE 1=1");
            query.AppendLine(" AND codigo_dre = @codigoDre AND ano_letivo = @anoLetivo");
            
            if (!string.IsNullOrEmpty(filtro.DataFrequencia))
            {                
                query.AppendLine(" AND DATE(data_aula) = @dataFrequencia");
            }

            var retorno = new PaginacaoResultadoDto<RegistroFrequenciaDiariaUeDto>();

            using (var multi = await contextoConsulta.Conexao.QueryMultipleAsync(query.ToString(), new { codigoDre = filtro.CodigoDre, anoLetivo = filtro.AnoLetivo, dataFrequencia }))
            {
                retorno.Items = (await multi.ReadAsync<RegistroFrequenciaDiariaUeDto>()).ToList();
                retorno.TotalRegistros = paginacao.QuantidadeRegistros <= 1 ? 1 : multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = paginacao.QuantidadeRegistros > 0
                ? (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros)
                : 1; 

            return retorno;
        }

        public async Task<PaginacaoResultadoDto<RegistroFrequenciaDiariaTurmaDto>> ObterFrequenciaDiariaPorUe(FiltroFrequenciaDiariaUeDto filtro)
        {
            var paginacao = filtro.ObterPaginacao();
            DateTime.TryParse(filtro.DataFrequencia, out var dataFrequencia);

            var query = new StringBuilder();

            query.AppendLine("SELECT");
            query.AppendLine("   data_aula AS Data");
            query.AppendLine(" , turma");
            query.AppendLine(" , total_alunos AS QuantidadeAlunos");
            query.AppendLine(" , total_presencas AS QuantidadeAlunosPresentes");
            query.AppendLine(" , percentual AS PercentualFrequencia");
            query.AppendLine(" , nivel_frequencia AS NivelFrequencia");
            query.AppendLine("FROM painel_educacional_consolidacao_frequencia_diaria_ue_teste");
            query.AppendLine("WHERE codigo_ue = @codigoUe AND ano_letivo = @anoLetivo");

            if (!string.IsNullOrEmpty(filtro.DataFrequencia))
            {
                query.AppendLine(" AND DATE(data_aula) = @dataFrequencia");
            }

            if (paginacao.QuantidadeRegistros > 0)
            {
                query.AppendFormat("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros);
            }
            query.AppendLine("; SELECT COUNT(id) FROM painel_educacional_consolidacao_frequencia_diaria_ue_teste WHERE 1=1");
            query.AppendLine("  AND codigo_ue = @codigoUe AND ano_letivo = @anoLetivo");

            if (!string.IsNullOrEmpty(filtro.DataFrequencia))
            {
                query.AppendLine(" AND codigo_ue = @codigoUe AND DATE(data_aula) = @dataFrequencia");
            }

            var retorno = new PaginacaoResultadoDto<RegistroFrequenciaDiariaTurmaDto>();

            using (var multi = await contextoConsulta.Conexao.QueryMultipleAsync(query.ToString(), new { codigoUe = filtro.CodigoUe, anoLetivo =  filtro.AnoLetivo, dataFrequencia }))
            {
                retorno.Items = (await multi.ReadAsync<RegistroFrequenciaDiariaTurmaDto>()).ToList();
                retorno.TotalRegistros = paginacao.QuantidadeRegistros <= 1 ? 1 : multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = paginacao.QuantidadeRegistros > 0
                ? (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros)
                : 1;

            return retorno;
        }
    }
}
