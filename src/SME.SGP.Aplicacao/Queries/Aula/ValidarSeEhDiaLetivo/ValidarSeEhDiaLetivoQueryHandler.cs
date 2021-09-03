using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ValidarSeEhDiaLetivoQueryHandler : IRequestHandler<ValidarSeEhDiaLetivoQuery, bool>
    {
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioEvento repositorioEvento;
        private readonly IServicoUsuario servicoUsuario;

        public ValidarSeEhDiaLetivoQueryHandler(IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioEvento repositorioEvento, IServicoUsuario servicoUsuario)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioEvento = repositorioEvento ?? throw new ArgumentNullException(nameof(repositorioEvento));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            
        }

        public async Task<bool> Handle(ValidarSeEhDiaLetivoQuery request, CancellationToken cancellationToken)
        {
            DateTime dataInicial = request.DataInicio.Date;
            DateTime dataFinal = request.DataInicio.Date;
            var periodoEscolar = await repositorioPeriodoEscolar.ObterPorTipoCalendarioData(request.TipoCalendarioId, dataInicial, dataFinal);
            if (periodoEscolar == null)
                return false;

            var eventos = await repositorioEvento.ObterEventosCalendarioPorDia(request.TipoCalendarioId, request.DreId, request.UeId, request.DataInicio, true, true);

            bool eventoLetivoDia = false;
            eventoLetivoDia = ExisteEventoLetivoNoDia(eventos, eventoLetivoDia);

            // Se eh dia da semana e não existe evento não letivo no dia (true)
            if (!ValidaSeEhFinalSemana(dataInicial, dataFinal) && eventoLetivoDia == false)
                return true;
            // eh final de semana com evento letivo (true)
            else if (ValidaSeEhFinalSemana(dataInicial, dataFinal) && eventoLetivoDia == true)
                return true;

            return false;
        }

        private static bool ExisteEventoLetivoNoDia(IEnumerable<Dominio.Evento> eventos, bool eventoLetivoDia)
        {
            foreach (var evento in eventos)
            {
                if (evento.Letivo == Dominio.EventoLetivo.Sim)
                    eventoLetivoDia = true;
            }

            return eventoLetivoDia;
        }

        private bool ValidaSeEhFinalSemana(DateTime inicio, DateTime fim)
        {
            for (DateTime data = inicio; data <= fim; data = data.AddDays(1))
                if (data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday)
                    return true;
            return false;
        }
    }
}
