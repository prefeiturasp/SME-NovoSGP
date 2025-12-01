using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA.ObterNovosEncaminhamentosNAAPAPorTipo
{
    public class ObterNovosEncaminhamentosNAAPAPorTipoQueryHandler : ConsultasBase,
        IRequestHandler<ObterNovosEncaminhamentosNAAPAPorTipoQuery, PaginacaoResultadoDto<NovoEncaminhamentoNAAPAResumoDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNAAPA;

        public ObterNovosEncaminhamentosNAAPAPorTipoQueryHandler(
            IContextoAplicacao contextoAplicacao,
            IMediator mediator,
            IRepositorioNovoEncaminhamentoNAAPA repositorioNovoEncaminhamentoNAAPA) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioNovoEncaminhamentoNAAPA = repositorioNovoEncaminhamentoNAAPA ??
                throw new ArgumentNullException(nameof(repositorioNovoEncaminhamentoNAAPA));
        }

        public async Task<PaginacaoResultadoDto<NovoEncaminhamentoNAAPAResumoDto>> Handle(
            ObterNovosEncaminhamentosNAAPAPorTipoQuery request,
            CancellationToken cancellationToken)
        {
            var turmas = Enumerable.Empty<AbrangenciaTurmaRetorno>();

            if (!string.IsNullOrEmpty(request.CodigoUe))
            {
                if (request.TurmaId > 0)
                    turmas = new List<AbrangenciaTurmaRetorno>() { new() { Id = request.TurmaId } };
                else
                    turmas = await mediator.Send(
                        new ObterAbrangenciaTurmasPorUeModalidadePeriodoHistoricoAnoLetivoTiposQuery(
                            request.CodigoUe, 0, 0, request.ExibirHistorico, request.AnoLetivo, null, true));
            }

            var turmasIds = turmas.NaoEhNulo() && turmas.Any() ? turmas.Select(s => s.Id) : null;

            var resultado = await repositorioNovoEncaminhamentoNAAPA.ListarPaginado(
                request.AnoLetivo,
                request.DreId,
                request.CodigoUe,
                request.CodigoNomeAluno,
                request.DataAberturaQueixaInicio,
                request.DataAberturaQueixaFim,
                request.Situacao,
                request.Prioridade,
                turmasIds?.ToArray() ?? Array.Empty<long>(),
                Paginacao,
                request.ExibirEncerrados,
                request.Ordenacao);

            return await MapearParaDto(resultado);
        }

        private async Task<PaginacaoResultadoDto<NovoEncaminhamentoNAAPAResumoDto>> MapearParaDto(
            PaginacaoResultadoDto<NovoEncaminhamentoNAAPAResumoDto> resultadoDto)
        {
            return new PaginacaoResultadoDto<NovoEncaminhamentoNAAPAResumoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = await MapearItensParaDto(resultadoDto.Items)
            };
        }

        private async Task<IEnumerable<NovoEncaminhamentoNAAPAResumoDto>> MapearItensParaDto(
            IEnumerable<NovoEncaminhamentoNAAPAResumoDto> encaminhamentos)
        {
            var listaEncaminhamentos = new List<NovoEncaminhamentoNAAPAResumoDto>();

            foreach (var encaminhamento in encaminhamentos)
            {
                listaEncaminhamentos.Add(new NovoEncaminhamentoNAAPAResumoDto()
                {
                    Id = encaminhamento.Id,
                    TipoQuestionario = ((TipoQuestionario)int.Parse(encaminhamento.TipoQuestionario)).Name(),
                    UeNome = encaminhamento.UeNome,
                    NomeAluno = encaminhamento.NomeAluno,
                    TurmaNome = encaminhamento.TurmaNome,
                    DataAberturaQueixaInicio = encaminhamento.DataAberturaQueixaInicio,
                    DataUltimoAtendimento = encaminhamento.DataUltimoAtendimento,
                    Situacao = ((SituacaoNAAPA)int.Parse(encaminhamento.Situacao)).Name(),
                    SuspeitaViolencia = encaminhamento.SuspeitaViolencia
                });
            }

            return await Task.FromResult(listaEncaminhamentos);
        }
    }
}