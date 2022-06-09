using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasDaTurmaPorTipoCalendarioQueryHandler : IRequestHandler<ObterAulasDaTurmaPorTipoCalendarioQuery, IEnumerable<Dominio.AulaInformacoesAdicionais>>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;

        public ObterAulasDaTurmaPorTipoCalendarioQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<IEnumerable<Dominio.AulaInformacoesAdicionais>> Handle(ObterAulasDaTurmaPorTipoCalendarioQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAula.ObterAulasPorTurmaETipoCalendario(request.TipoCalendarioId, request.TurmaId, request.CriadoPor);
        }
    }
}
