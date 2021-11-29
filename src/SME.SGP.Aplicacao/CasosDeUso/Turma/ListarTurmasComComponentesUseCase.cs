using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarTurmasComComponentesUseCase : ConsultasBase,  IListarTurmasComComponentesUseCase
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

            return MapearParaDtoComPaginacao(await mediator.Send(new ListagemTurmasComComponenteQuery(filtroTurmaDto.UeCodigo, filtroTurmaDto.DreCodigo,
                                                                                           filtroTurmaDto.TurmaCodigo, filtroTurmaDto.AnoLetivo,
                                                                                           qtdeRegistros, qtdeRegistrosIgnorados, filtroTurmaDto.Bimestre, filtroTurmaDto.Modalidade)));
        }

        private PaginacaoResultadoDto<ListaTurmasComComponenteDto> MapearParaDtoComPaginacao(PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto> turmasPaginadas)
        {
            if (turmasPaginadas == null)
            {
                turmasPaginadas = new PaginacaoResultadoDto<RetornoConsultaListagemTurmaComponenteDto>();
            }
            return new PaginacaoResultadoDto<ListaTurmasComComponenteDto>
            {
                Items = MapearEventosParaDto(turmasPaginadas.Items),
                TotalPaginas = turmasPaginadas.TotalPaginas,
                TotalRegistros = turmasPaginadas.TotalRegistros
            };
        }

        private IEnumerable<ListaTurmasComComponenteDto> MapearEventosParaDto(IEnumerable<RetornoConsultaListagemTurmaComponenteDto> items)
        {
            return items?.Select(c => MapearParaDto(c));
        }

        private ListaTurmasComComponenteDto MapearParaDto(RetornoConsultaListagemTurmaComponenteDto turmas)
        {
            return turmas == null ? null : new ListaTurmasComComponenteDto
            {
                Id = turmas.Id,
                NomeTurma = turmas.NomeTurmaFormatado(),//$"{turmas.Modalidade} - {turmas.NomeTurma} - {turmas.Ano} - {turmas.NomeComponenteCurricular}",
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
