using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiasForaDoPeriodoEscolarQueryHandler : IRequestHandler<ObterDiasForaDoPeriodoEscolarQuery, IEnumerable<DateTime>>
    {
        public async Task<IEnumerable<DateTime>> Handle(ObterDiasForaDoPeriodoEscolarQuery request, CancellationToken cancellationToken)
        {
            var hoje = DateTime.Today;
            var primeiroDiaAno = new DateTime(hoje.Year, 01, 01);
            var ultimoDiaAno = new DateTime(hoje.Year, 12, 31);

            var diasForaDoPeriodo = new List<DateTime>();

            for (DateTime dia = primeiroDiaAno; dia <= ultimoDiaAno; dia = dia.AddDays(1))
            {
                var encontrouDiaNoPeriodo = false;
                foreach (var periodo in request.PeriodosEscolares.OrderBy(c => c.Bimestre))
                {
                    if ((periodo.PeriodoInicio.Date <= dia) && (dia <= periodo.PeriodoFim.Date))
                        encontrouDiaNoPeriodo = true;
                }
                if (!encontrouDiaNoPeriodo)
                    diasForaDoPeriodo.Add(dia);
            }
            return await Task.FromResult(diasForaDoPeriodo);
        }
    }
}