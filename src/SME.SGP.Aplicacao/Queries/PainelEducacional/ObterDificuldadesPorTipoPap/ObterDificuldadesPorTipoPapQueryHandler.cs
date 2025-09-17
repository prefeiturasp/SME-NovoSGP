using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDificuldadesPorTipoPap
{
    public class ObterDificuldadesPorTipoPapQueryHandler : IRequestHandler<ObterDificuldadesPorTipoPapQuery, ContagemDificuldadePorTipoDto>
    {
        private readonly IRepositorioPapConsulta repositorioPapConsulta;

        public ObterDificuldadesPorTipoPapQueryHandler(IRepositorioPapConsulta repositorioPapConsulta)
        {
            this.repositorioPapConsulta = repositorioPapConsulta ?? throw new ArgumentNullException(nameof(repositorioPapConsulta));
        }

        public async Task<ContagemDificuldadePorTipoDto> Handle(ObterDificuldadesPorTipoPapQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPapConsulta.ObterContagemDificuldadesPorTipo(request.TipoPap);
        }
    }
}