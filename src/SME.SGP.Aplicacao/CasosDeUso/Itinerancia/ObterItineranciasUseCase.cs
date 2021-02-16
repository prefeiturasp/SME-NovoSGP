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
            var listaItinerancias = new List<ItineranciaResumoDto>();

            var CodigosAluno = itinerancias.Where(a => a.AlunoCodigo != null).Select(a => a.AlunoCodigo).Distinct().ToArray();

            try
            {
                var alunosEol = await mediator.Send(new ObterAlunosEolPorCodigosEAnoQuery(CodigosAluno.Select(long.Parse).ToArray(), anoLetivo));
                var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(alunosEol.Select(al => al.CodigoTurma.ToString()).ToArray()));

                foreach (var itinerancia in itinerancias)
                {
                    
                    var aluno = alunosEol.FirstOrDefault(a => a.CodigoAluno == int.Parse(itinerancia.AlunoCodigo?? : 0));
                    listaItinerancias.Add(new ItineranciaResumoDto()
                    {
                        Id = itinerancia.Id,
                        DataVisita = itinerancia.DataVisita,
                        Situacao = itinerancia.Situacao != 0 ? itinerancia.Situacao.Name() : "",
                        TurmaNome = $"{itinerancia.ModalidadeCodigo.ShortName()} - {itinerancia.TurmaNome}",
                        EstudanteNome = $"{aluno.NumeroAlunoChamada} - {aluno.NomeAluno}",
                        UeNome = $"{itinerancia.TipoEscola.ShortName()} - {itinerancia.UeNome}"

                    });
                }

                return listaItinerancias;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }

        private string OberterNomeTurmaFormatado(Turma turma)
        {
            var turmaNome = "";

            if (turma != null)
                turmaNome = $"{turma.ModalidadeCodigo.ShortName()} - {turma.Nome}";

            return turmaNome;
        }      
    }
}
