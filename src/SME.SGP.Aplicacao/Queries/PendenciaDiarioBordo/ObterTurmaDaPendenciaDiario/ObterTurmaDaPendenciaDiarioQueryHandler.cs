using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmaDaPendenciaDiarioQueryHandler : IRequestHandler<ObterTurmaDaPendenciaDiarioQuery, Turma>
    {
        private readonly IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordoConsulta;

        public ObterTurmaDaPendenciaDiarioQueryHandler(IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordoConsulta)
        {
            this.repositorioPendenciaDiarioBordoConsulta = repositorioPendenciaDiarioBordoConsulta ?? throw new ArgumentNullException(nameof(repositorioPendenciaDiarioBordoConsulta));
        }

        public async Task<Turma> Handle(ObterTurmaDaPendenciaDiarioQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaDiarioBordoConsulta.ObterTurmaPorPendenciaDiario(request.PendenciaId);
    }
}
