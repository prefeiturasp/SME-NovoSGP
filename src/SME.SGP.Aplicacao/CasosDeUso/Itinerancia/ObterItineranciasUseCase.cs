using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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
            return await MapearParaDto(await mediator.Send(new ObterItineranciasQuery(filtro.DreId,
                                                                                      filtro.UeId,
                                                                                      filtro.TurmaId,
                                                                                      filtro.AnoLetivo,
                                                                                      filtro.AlunoCodigo,
                                                                                      filtro.DataInicio,
                                                                                      filtro.DataFim,
                                                                                      filtro.Situacao)), filtro.AnoLetivo);
        }

        private async Task<PaginacaoResultadoDto<ItineranciaResumoDto>> MapearParaDto(PaginacaoResultadoDto<ItineranciaRetornoDto> resultadoDto, int anoLetivo)
        {
            return new PaginacaoResultadoDto<ItineranciaResumoDto>()
            {
                TotalPaginas = resultadoDto.TotalPaginas,
                TotalRegistros = resultadoDto.TotalRegistros,
                Items = await MapearParaDto(resultadoDto.Items, anoLetivo)
            };
        }

        private async Task<IEnumerable<ItineranciaResumoDto>> MapearParaDto(IEnumerable<ItineranciaRetornoDto> itinerancias, int anoLetivo)
        {
            var itineranciasParaRetornar = new List<ItineranciaResumoDto>();

            var itineranciasAgrupadas = itinerancias.GroupBy(i => i.Id);

            var codigosAluno = itinerancias.Where(a => !string.IsNullOrEmpty(a.AlunoCodigo)).Select(a => long.Parse(a.AlunoCodigo)).Distinct().ToArray();

            var alunosEol = await mediator.Send(new ObterAlunosEolPorCodigosEAnoQuery(codigosAluno, anoLetivo));
            var codigosDasTurmas = alunosEol.Select(al => al.CodigoTurma.ToString()).Distinct().ToArray();

            var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(codigosDasTurmas));

            foreach (var itineranciasAgrupada in itineranciasAgrupadas)
            {
                var itineranciaParaTratar = itineranciasAgrupada.FirstOrDefault();
                var itineranciaDto = new ItineranciaResumoDto();
                itineranciaDto.DataVisita = itineranciaParaTratar.DataVisita;
                itineranciaDto.UeNome = ObterNomeUe(itineranciasAgrupada, itineranciaParaTratar);
                itineranciaDto.Id = itineranciasAgrupada.Key;
                itineranciaDto.Situacao = itineranciaParaTratar.Situacao.Name();
                var estudanteInfos = ObterEstudanteNomeCodigo(itineranciaParaTratar, alunosEol, itineranciasAgrupada);
                itineranciaDto.EstudanteNome = estudanteInfos.Item1;
                itineranciaDto.TurmaNome = ObterTurmaNome(estudanteInfos.Item2, turmas);

                itineranciasParaRetornar.Add(itineranciaDto);

            }
            return itineranciasParaRetornar;
          
        }

        private string ObterTurmaNome(string item2, IEnumerable<Turma> turmas)
        {
            if (string.IsNullOrEmpty(item2))
                return string.Empty;
            else
            {
                var turma = turmas.FirstOrDefault(a => a.CodigoTurma == item2);
                return turma is null ?  string.Empty : turma.NomeComModalidade();
            }
        }

        private (string, string) ObterEstudanteNomeCodigo(ItineranciaRetornoDto itineranciaParaTratar, IEnumerable<TurmasDoAlunoDto> alunosEol, IGrouping<long, ItineranciaRetornoDto> itineranciasAgrupada)
        {
            if (itineranciasAgrupada.Any(a => !string.IsNullOrEmpty(a.AlunoCodigo)))
            {

                var registrosDiferentes = itineranciasAgrupada.Select(a => a.AlunoCodigo).Distinct();
                var qntRegistros = registrosDiferentes.Count();
                if (qntRegistros > 1)
                    return ($"{qntRegistros} registros selecionados.", string.Empty);
                else {
                    var alunoCodigo = itineranciaParaTratar.AlunoCodigo;
                    var alunoEol = alunosEol.FirstOrDefault(a => a.CodigoAluno == int.Parse(alunoCodigo));
                    return (alunoEol?.ObterNomeComNumeroChamada(), alunoEol.CodigoTurma.ToString()); 
                }

            }
            else return ("Sem informação", string.Empty);
        }

        private string ObterNomeUe(IGrouping<long, ItineranciaRetornoDto> itineranciasAgrupada, ItineranciaRetornoDto itineranciaDto)
        {
            if (itineranciasAgrupada.Any(a => !string.IsNullOrEmpty(a.UeNome)))
            {
                var registrosDiferentes = itineranciasAgrupada.Select(a => a.UeId).Distinct();
                var qntRegistros = registrosDiferentes.Count();
                if (qntRegistros > 1)
                    return $"{qntRegistros} registros selecionados.";
                else return $"{itineranciaDto.TipoEscola.ShortName()} {itineranciaDto.UeNome}";
            }
            return "Sem informação";
        }

     
    }
}
