using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasPendenciaFechamento : ConsultasBase, IConsultasPendenciaFechamento
    {
        private readonly IRepositorioPendenciaFechamento repositorioPendenciaFechamento;
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        
        public ConsultasPendenciaFechamento(IContextoAplicacao contextoAplicacao
                                , IRepositorioPendenciaFechamento repositorioPendenciaFechamento,
                        IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular) : base(contextoAplicacao)
        {
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
        }

        public async Task<PaginacaoResultadoDto<PendenciaFechamentoResumoDto>> Listar(FiltroPendenciasFechamentosDto filtro)
        {
            var retornoConsultaPaginada = await repositorioPendenciaFechamento.ListarPaginada(Paginacao, filtro.TurmaCodigo, filtro.Bimestre, filtro.ComponenteCurricularId);

            if (retornoConsultaPaginada.Items != null && retornoConsultaPaginada.Items.Any())
            {
                // Atualiza nome da situacao
                retornoConsultaPaginada.Items.ToList()
                    .ForEach(i => i.SituacaoNome = Enum.GetName(typeof(SituacaoPendencia), i.Situacao));

                // Carrega nomes das disciplinas para o DTO de retorno
                var disciplinasEOL = await repositorioComponenteCurricular.ObterDisciplinasPorIds(retornoConsultaPaginada.Items.Select(a => a.DisciplinaId).Distinct().ToArray());
                foreach(var disciplinaEOL in disciplinasEOL)
                {
                    retornoConsultaPaginada.Items.Where(c => c.DisciplinaId == disciplinaEOL.CodigoComponenteCurricular).ToList()
                        .ForEach(d => d.ComponenteCurricular = disciplinaEOL.Nome);
                }
            }

            return retornoConsultaPaginada;
        }

        public async Task<PendenciaFechamentoCompletoDto> ObterPorPendenciaId(long pendenciaId)
        {
            var pendencia = await repositorioPendenciaFechamento.ObterPorPendenciaId(pendenciaId);
            if (pendencia == null)
                throw new NegocioException("Pendencia informada não localizada.");

            pendencia.SituacaoNome = Enum.GetName(typeof(SituacaoPendencia), pendencia.Situacao);

            var disciplinasEOL = await repositorioComponenteCurricular.ObterDisciplinasPorIds(new long[] { pendencia.DisciplinaId });
            if (disciplinasEOL == null || !disciplinasEOL.Any())
                throw new NegocioException("Componente curricular informado não localizado.");

            var disciplinaEOL = disciplinasEOL.First();
            pendencia.ComponenteCurricular = disciplinaEOL.Nome;
            return pendencia;
        }
    }
}
