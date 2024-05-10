using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDiasForaDoPeriodoEscolarQueryHandler : IRequestHandler<ObterDiasForaDoPeriodoEscolarQuery, IEnumerable<DateTime>>
    {
        private readonly IRepositorioCache repositorioCache;

        public ObterDiasForaDoPeriodoEscolarQueryHandler(IRepositorioCache repositorioCache)
        {
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task<IEnumerable<DateTime>> Handle(ObterDiasForaDoPeriodoEscolarQuery request, CancellationToken cancellationToken)
        {
            var chaveCache = string.Format(NomeChaveCache.DIAS_FORA_PERIODO_ESCOLAR_IDS_CONSIDERADOS, string.Join(",", request.PeriodosEscolares.Select(p => p.Id)));

            return await repositorioCache
                .ObterAsync(chaveCache, async
                    () => await MontarListaDiasForaPeriodo(request.PeriodosEscolares), minutosParaExpirar: 300);
        }

        private static async Task<IEnumerable<DateTime>> MontarListaDiasForaPeriodo(IEnumerable<PeriodoEscolar> periodosEscolares)
        {
            var hoje = DateTime.Today;
            var primeiroDiaAno = new DateTime(hoje.Year, 01, 01);
            var ultimoDiaAno = new DateTime(hoje.Year, 12, 31);

            var diasForaDoPeriodo = new List<DateTime>();

            for (DateTime dia = primeiroDiaAno; dia <= ultimoDiaAno; dia = dia.AddDays(1))
            {
                var encontrouDiaNoPeriodo = false;
                foreach (var periodo in periodosEscolares.OrderBy(c => c.Bimestre))
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