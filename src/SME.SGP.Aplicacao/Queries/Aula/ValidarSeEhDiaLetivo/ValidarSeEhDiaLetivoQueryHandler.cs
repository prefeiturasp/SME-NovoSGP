using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ValidarSeEhDiaLetivoQueryHandler : IRequestHandler<ValidarSeEhDiaLetivoQuery, bool>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;

        public ValidarSeEhDiaLetivoQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
        }

        public async Task<bool> Handle(ValidarSeEhDiaLetivoQuery request, CancellationToken cancellationToken)
        {
            DateTime dataInicial = request.DataInicio.Date;
            DateTime dataFinal = request.DataFim.Date;
            var periodoEscolar = await repositorioPeriodoEscolar.ObterPorTipoCalendarioData(request.TipoCalendarioId, dataInicial, dataFinal);
            if (periodoEscolar == null)
                return false;

            if (request.EhLetivo && request.TipoEventoId != (int)TipoEvento.LiberacaoExcepcional)
                return ValidaSeEhFinalSemana(dataInicial, dataFinal);

            return true;
        }

        private bool ValidaSeEhFinalSemana(DateTime inicio, DateTime fim)
        {
            for (DateTime data = inicio; data <= fim; data = data.AddDays(1))
                if (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                    return false;
            return true;
        }
    }
}
