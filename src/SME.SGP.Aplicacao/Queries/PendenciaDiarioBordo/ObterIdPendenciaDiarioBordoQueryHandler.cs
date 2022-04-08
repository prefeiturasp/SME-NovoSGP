using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PendenciaDiarioBordo
{
    public class ObterIdPendenciaDiarioBordoQueryHandler : IRequestHandler<ObterIdPendenciaDiarioBordoQuery, bool>
    {
        private readonly IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordoConsulta;

        public ObterIdPendenciaDiarioBordoQueryHandler(IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordoConsulta)
        {
            this.repositorioPendenciaDiarioBordoConsulta = repositorioPendenciaDiarioBordoConsulta ?? throw new ArgumentNullException(nameof(repositorioPendenciaDiarioBordoConsulta));
        }

        public async Task<bool> Handle(ObterIdPendenciaDiarioBordoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPendenciaDiarioBordoConsulta.ExisteIdPendenciaDiarioBordo(request.PendenciaID);

        }
    }
}
