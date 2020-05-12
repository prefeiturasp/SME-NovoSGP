using SME.SGP.Infra;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoDeCalendarioDaTurmaUseCase
    {
         public static async Task<TipoCalendarioSugestaoDto> Executar(IMediator mediator, ObterTipoDeCalendarioDaTurmaEntrada obterTipoDeCalendarioDaTurmaEntrada)
        {

            Turma turma = await mediator.Send(new ObterTurmaPorCodigoQuery() {  TurmaCodigo = obterTipoDeCalendarioDaTurmaEntrada.TurmaCodigo }) as Turma;
            if (turma == null)
                throw new NegocioException($"Não foi encontrado a turma {obterTipoDeCalendarioDaTurmaEntrada.TurmaCodigo}.");

            var tipoDeCalendario = await mediator.Send(new ObterTipoDeCalendarioDaTurmaQuery() { Turma = turma });
            if (tipoDeCalendario == null)
                throw new NegocioException("Não foi encontrado tipo de calendário para esta turma.");

            return new TipoCalendarioSugestaoDto() { Id = tipoDeCalendario.Id, Nome = tipoDeCalendario.Nome };
            
        }

    }
}
