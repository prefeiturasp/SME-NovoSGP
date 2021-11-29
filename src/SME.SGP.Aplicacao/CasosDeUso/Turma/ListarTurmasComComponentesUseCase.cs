using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarTurmasComComponentesUseCase : ConsultasBase, IListarTurmasComComponentesUseCase
    {
        private readonly IMediator mediator;

        public ListarTurmasComComponentesUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<PaginacaoResultadoDto<ListaTurmasComComponenteDto>> Executar(FiltroTurmaDto filtroTurmaDto)
        {
            var resultado = new PaginacaoResultadoDto<ListaTurmasComComponenteDto>();
            int qtdeRegistros = Paginacao.QuantidadeRegistros;
            int qtdeRegistrosIgnorados = Paginacao.QuantidadeRegistrosIgnorados;

            var turmasPaginadas = await mediator.Send(new ListagemTurmasComComponenteQuery(filtroTurmaDto.UeCodigo, filtroTurmaDto.DreCodigo,
                                                                                           filtroTurmaDto.TurmaCodigo, filtroTurmaDto.AnoLetivo,
                                                                                           qtdeRegistros, qtdeRegistrosIgnorados, filtroTurmaDto.Bimestre, filtroTurmaDto.Modalidade.Value, filtroTurmaDto.Semestre));
                                                                                           


            var componentesCodigos = turmasPaginadas.Items.Select(c => c.ComponenteCurricularCodigo).Distinct().ToArray();

            var componentesRetorno = await mediator.Send(new ObterDescricaoComponentesCurricularesPorIdsQuery(componentesCodigos));

            return MapearParaDtoComPaginacao(turmasPaginadas, componentesRetorno);
        }

        private PaginacaoResultadoDto<ListaTurmasComComponenteDto> MapearParaDtoComPaginacao(PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto> turmasPaginadas, IEnumerable<ComponenteCurricularSimplesDto> listaComponentes)
        {
            if (turmasPaginadas == null)
            {
                turmasPaginadas = new PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>();
            }
            return new PaginacaoResultadoDto<ListaTurmasComComponenteDto>
            {
                Items = MapearEventosParaDto(turmasPaginadas.Items, listaComponentes),
                TotalPaginas = turmasPaginadas.TotalPaginas,
                TotalRegistros = turmasPaginadas.TotalRegistros
            };
        }

        private IEnumerable<ListaTurmasComComponenteDto> MapearEventosParaDto(IEnumerable<RetornoConsultaListagemTurmaComponenteDto> items, IEnumerable<ComponenteCurricularSimplesDto> listaComponentes)
        {
            return items?.Select(c => MapearParaDto(c, listaComponentes));
        }

        private ListaTurmasComComponenteDto MapearParaDto(RetornoConsultaListagemTurmaComponenteDto turmas, IEnumerable<ComponenteCurricularSimplesDto> listaComponentes)
        {
            var nomeComponente = listaComponentes.FirstOrDefault(c => c.Id == turmas.ComponenteCurricularCodigo).Descricao;
            return turmas == null ? null : new ListaTurmasComComponenteDto
            {
                Id = turmas.Id,
                NomeTurma = turmas.NomeTurmaFormatado(nomeComponente),
                TurmaCodigo = turmas.TurmaCodigo,
                ComponenteCurricularCodigo = turmas.ComponenteCurricularCodigo,
                Turno = ObterTipoTurnoTurma(turmas.Turno)
            };
        }

        private string ObterTipoTurnoTurma(int tipoTurno)
        {
            var nomeTipoTurno = Enum.GetValues(typeof(TipoTurnoEOL))
                                .Cast<TipoTurnoEOL>()
                                .Where(d => ((int)d) == tipoTurno)
                                .Select(d => new { Name = d.Name() })
                                .FirstOrDefault();
            return nomeTipoTurno.Name;
        }
    }
}
