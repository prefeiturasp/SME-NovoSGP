using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterItineranciasUseCase : AbstractUseCase, IObterItineranciasUseCase
    {
        public ObterItineranciasUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<ItineranciaResumoDto>> Executar(FiltroPesquisaItineranciasDto filtro)
        {
            var listaRetorno = await mediator.Send(new ObterItineranciasQuery(filtro.DreId,
                                                                                      filtro.UeId,
                                                                                      filtro.TurmaId,
                                                                                      filtro.AnoLetivo,
                                                                                      filtro.AlunoCodigo,
                                                                                      filtro.DataInicio,
                                                                                      filtro.DataFim,
                                                                                      filtro.Situacao,
                                                                                      filtro.CriadoRf));

            if (listaRetorno != null && listaRetorno.Items.Any())
            {
                return await MapearParaDto(listaRetorno, filtro.AnoLetivo);
            }

            return default;
        }

        private async Task<PaginacaoResultadoDto<ItineranciaResumoDto>> MapearParaDto(PaginacaoResultadoDto<ItineranciaRetornoQueryDto> resultadoDto, int anoLetivo)
        {
            return new PaginacaoResultadoDto<ItineranciaResumoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = await MapearParaDto(resultadoDto.Items, anoLetivo)
            };
        }

        private async Task<IEnumerable<ItineranciaResumoDto>> MapearParaDto(IEnumerable<ItineranciaRetornoQueryDto> itinerancias, int anoLetivo)
        {
            var itineranciasParaRetornar = new List<ItineranciaResumoDto>();

            if (itinerancias == null || !itinerancias.Any())
                return itineranciasParaRetornar;

            var alunosEol = new List<TurmasDoAlunoDto>();
            var itineranciasAlunos = new List<ItineranciaCodigoAlunoDto>();
            var turmas = new List<Turma>();

            if (itinerancias.Any(a => a.Alunos == 1))
            {
                var itineranciasIdsComAlunos = itinerancias.Where(a => a.Alunos == 1).Select(a => a.Id).ToArray();

                itineranciasAlunos = (await mediator.Send(new ObterAlunosCodigoPorItineranciasQuery(itineranciasIdsComAlunos))).ToList();
                var alunosCodigos = itineranciasAlunos.Select(a => a.AlunoCodigo).ToArray();

                alunosEol = (await mediator.Send(new ObterAlunosEolPorCodigosQuery(alunosCodigos))).ToList();

                var codigosDasTurmas = alunosEol.Select(al => al.CodigoTurma.ToString()).Distinct().ToArray();
                turmas = (await mediator.Send(new ObterTurmasPorCodigosQuery(codigosDasTurmas))).ToList();
            }

            foreach (var item in itinerancias)
            {
                var itineranciaParaAdicionar = new ItineranciaResumoDto();

                itineranciaParaAdicionar.DataVisita = item.DataVisita.ToString("dd/MM/yyyy");
                itineranciaParaAdicionar.EstudanteNome = ObterEstudanteNomeCodigo(item, alunosEol, itineranciasAlunos);
                itineranciaParaAdicionar.Id = item.Id;
                itineranciaParaAdicionar.Situacao = item.Situacao.Name();
                itineranciaParaAdicionar.UeNome = await ObterNomeUeAsync(item.UeId);
                itineranciaParaAdicionar.TurmaNome = ObterTurmaNome(item, turmas, itineranciasAlunos);
                itineranciaParaAdicionar.CriadoPor = item.CriadoPor;

                itineranciasParaRetornar.Add(itineranciaParaAdicionar);
            }

            return itineranciasParaRetornar.OrderByDescending(i => DateTime.Parse(i.DataVisita)).ThenBy(i => i.UeNome).ThenBy(i => i.EstudanteNome);

        }

        private string ObterTurmaNome(ItineranciaRetornoQueryDto item, List<Turma> turmas, List<ItineranciaCodigoAlunoDto> itineranciasAlunos)
        {
            if (item.Alunos > 1)
                return $"{item.Alunos} registros selecionados.";
            else if (item.Alunos == 1)
            {
                var alunosDaItinerancia = itineranciasAlunos.FirstOrDefault(a => a.ItineranciaId == item.Id);
                return turmas.FirstOrDefault(a => a.Id == alunosDaItinerancia.TurmaId)?.NomeComModalidade();
            }
            else
            {
                return "Sem informação";
            }
        }

        private async Task<string> ObterNomeUeAsync(long ueId)
        {
            var ue = await mediator.Send(new ObterUePorIdQuery(ueId));
            if (ue == null)
                throw new NegocioException("Não foi possível encrontra a UE!");
            
            return $"{ue.TipoEscola.ShortName()} {ue.Nome}";
        }

        private string ObterEstudanteNomeCodigo(ItineranciaRetornoQueryDto itineranciaParaTratar, IEnumerable<TurmasDoAlunoDto> alunosEol, List<ItineranciaCodigoAlunoDto> itineranciaCodigoAlunos)
        {
            if (itineranciaParaTratar.Alunos > 1)
                return $"{itineranciaParaTratar.Alunos} registros selecionados.";
            else if (itineranciaParaTratar.Alunos == 1)
            {
                var alunosDaItinerancia = itineranciaCodigoAlunos.FirstOrDefault(a => a.ItineranciaId == itineranciaParaTratar.Id);
                return alunosEol.FirstOrDefault(a => a.CodigoAluno == alunosDaItinerancia.AlunoCodigo)?.ObterNomeComNumeroChamada();
            }
            else
            {
                return "Sem informação";
            }
        }
    }
}
