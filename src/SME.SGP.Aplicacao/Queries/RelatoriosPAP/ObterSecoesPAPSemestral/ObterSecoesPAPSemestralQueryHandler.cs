using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterSecoesPAPSemestralQueryHandler : IRequestHandler<ObterSecoesPAPSemestralQuery, IEnumerable<SecaoRelatorioSemestralPAP>>
    {
        private readonly IRepositorioSecaoRelatorioSemestralPAP repositorio;

        public ObterSecoesPAPSemestralQueryHandler(IRepositorioSecaoRelatorioSemestralPAP repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<IEnumerable<SecaoRelatorioSemestralPAP>> Handle(ObterSecoesPAPSemestralQuery request, CancellationToken cancellationToken)
        {
            return this.repositorio.ObterSecoes();
        }
    }
}
