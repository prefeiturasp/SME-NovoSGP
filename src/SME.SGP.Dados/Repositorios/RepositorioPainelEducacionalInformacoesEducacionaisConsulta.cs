using Dapper;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.InformacoesEducacionais;
using SME.SGP.Infra.Interfaces;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioPainelEducacionalInformacoesEducacionaisConsulta : IRepositorioPainelEducacionalInformacoesEducacionaisConsulta
    {
        private readonly ISgpContextConsultas contextoConsulta;

        public RepositorioPainelEducacionalInformacoesEducacionaisConsulta(ISgpContextConsultas contextoConsulta)
        {
            this.contextoConsulta = contextoConsulta ?? throw new ArgumentNullException(nameof(contextoConsulta));
        }

        public async Task<PaginacaoResultadoDto<RegistroInformacoesEducacionaisUeDto>> ObterInformacoesEducacionais(FiltroInformacoesEducacionais filtro)
        {
            var paginacao = filtro.ObterPaginacao();
            var query = new StringBuilder();

            query.AppendLine("SELECT");
            query.AppendLine("   ue");
            query.AppendLine(" , idep_anos_iniciais AS idepAnosIniciais");
            query.AppendLine(" , idep_anos_finais AS idepAnosFinais");
            query.AppendLine(" , ideb_anos_iniciais AS idebAnosIniciais");
            query.AppendLine(" , ideb_anos_finais AS idebAnosFinais");
            query.AppendLine(" , ideb_ensino_medio AS idebEnsinoMedio");
            query.AppendLine(" , percentual_frequencia_global AS percentualFrequenciaGlobal");
            query.AppendLine(" , quantidade_turmas_pap AS quantidadeTurmasPap");
            query.AppendLine(" , percentual_frequencia_alunos_pap AS percentualFrequenciaAlunosPap");
            query.AppendLine(" , quantidade_alunos_desistentes_abandono AS quantidadeAlunosDesistentesAbandono");
            query.AppendLine(" , quantidade_promocoes AS quantidadePromocoes");
            query.AppendLine(" , quantidade_retencoes_frequencia AS quantidadeRetencoesFrequencia");
            query.AppendLine(" , quantidade_retencoes_nota AS quantidadeRetencoesNota");
            query.AppendLine(" , quantidade_notas_abaixo_media AS quantidadeNotasAbaixoMedia");
            query.AppendLine("FROM painel_educacional_consolidacao_informacoes_educacionais");
            query.AppendLine("WHERE codigo_dre = @codigoDre AND ano_letivo = @anoLetivo");

            if (paginacao.QuantidadeRegistros > 0)
            {
                query.AppendFormat("OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY", paginacao.QuantidadeRegistrosIgnorados, paginacao.QuantidadeRegistros);
            }

            query.AppendLine("; SELECT COUNT(id) FROM painel_educacional_consolidacao_informacoes_educacionais WHERE codigo_dre = @codigoDre AND ano_letivo = @anoLetivo");

            var retorno = new PaginacaoResultadoDto<RegistroInformacoesEducacionaisUeDto>();

            using (var multi = await contextoConsulta.Conexao.QueryMultipleAsync(query.ToString(), new { codigoDre = filtro.CodigoDre, anoLetivo = filtro.AnoLetivo }))
            {
                retorno.Items = (await multi.ReadAsync<RegistroInformacoesEducacionaisUeDto>()).ToList();
                retorno.TotalRegistros = paginacao.QuantidadeRegistros <= 1 ? 1 : multi.ReadFirst<int>();
            }

            retorno.TotalPaginas = paginacao.QuantidadeRegistros > 0
                ? (int)Math.Ceiling((double)retorno.TotalRegistros / paginacao.QuantidadeRegistros)
                : 1; 

            return retorno;
        }
    }
}
