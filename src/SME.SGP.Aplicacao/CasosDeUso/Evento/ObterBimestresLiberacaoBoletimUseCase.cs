using MediatR;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestresLiberacaoBoletimUseCase : IObterBimestresLiberacaoBoletimUseCase
    {
        private readonly IMediator mediator;

        public ObterBimestresLiberacaoBoletimUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<int[]> Executar(string codigoTurma)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma));

            if (turma == null)
                throw new NegocioException("Turma não encontrada!");

            var tipoCalendarioId = await mediator.Send(new ObterTipoCalendarioIdPorTurmaQuery(turma));

            if (tipoCalendarioId <= 0)
                throw new NegocioException("Tipo calendário da turma não encontrado!");

            return await mediator.Send(new ObterBimestresEventoLiberacaoBoletimQuery(tipoCalendarioId, DateTime.Now));
        }
    }
}
