using MediatR;
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
        private readonly IMediator mediator;

        public ConsultasPendenciaFechamento(IContextoAplicacao contextoAplicacao
                                , IRepositorioPendenciaFechamento repositorioPendenciaFechamento,
                                                        IMediator mediator) : base(contextoAplicacao)
        {
            this.repositorioPendenciaFechamento = repositorioPendenciaFechamento ?? throw new ArgumentNullException(nameof(repositorioPendenciaFechamento));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<PendenciaFechamentoResumoDto>> Listar(FiltroPendenciasFechamentosDto filtro)
        {
            var retornoConsultaPaginada = await repositorioPendenciaFechamento.ListarPaginada(Paginacao, filtro.TurmaCodigo, filtro.Bimestre, filtro.ComponenteCurricularId);

            if (!retornoConsultaPaginada.Items.NaoEhNulo() || !retornoConsultaPaginada.Items.Any())
                return retornoConsultaPaginada;
            var idsDisciplinas = retornoConsultaPaginada.Items.Select(a => a.DisciplinaId).Distinct().ToArray();
            var disciplinasEOL = await mediator.Send(new ObterComponentesCurricularesPorIdsQuery(idsDisciplinas));
                
            var dictDisciplinas = disciplinasEOL.ToDictionary(d => d.CodigoComponenteCurricular, d => d.Nome);
                
            foreach (var item in retornoConsultaPaginada.Items)
            {
                item.SituacaoNome = Enum.GetName(typeof(SituacaoPendencia), item.Situacao);
                    
                if (dictDisciplinas.TryGetValue(item.DisciplinaId, out var nomeDisciplina))
                {
                    item.ComponenteCurricular = nomeDisciplina;
                }
            }

            return retornoConsultaPaginada;
        }

        public async Task<PendenciaFechamentoCompletoDto> ObterPorPendenciaId(long pendenciaId)
        {
            var pendencia = await repositorioPendenciaFechamento.ObterPorPendenciaId(pendenciaId);
            if (pendencia.EhNulo())
                throw new NegocioException("Pendencia informada não localizada.");

            pendencia.SituacaoNome = Enum.GetName(typeof(SituacaoPendencia), pendencia.Situacao);

            var disciplinaEOL = await mediator.Send(new ObterComponenteCurricularPorIdQuery(pendencia.DisciplinaId));
            if (disciplinaEOL.EhNulo())
                throw new NegocioException("Componente curricular informado não localizado.");

            pendencia.ComponenteCurricular = disciplinaEOL.Nome;
            return pendencia;
        }
    }
}
