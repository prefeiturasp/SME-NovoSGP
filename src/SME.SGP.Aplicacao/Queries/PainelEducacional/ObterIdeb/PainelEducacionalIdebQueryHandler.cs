using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdeb
{
    public class PainelEducacionalIdebQueryHandler : IRequestHandler<PainelEducacionalIdebQuery, IEnumerable<PainelEducacionalIdebDto>>
    {
        private readonly IRepositorioPainelEducacionalIdeb repositorioPainelEducacionalIdeb;

        public PainelEducacionalIdebQueryHandler(IRepositorioPainelEducacionalIdeb repositorioPainelEducacionalIdeb)
        {
            this.repositorioPainelEducacionalIdeb = repositorioPainelEducacionalIdeb ?? throw new ArgumentNullException(nameof(repositorioPainelEducacionalIdeb));
        }

        public async Task<IEnumerable<PainelEducacionalIdebDto>> Handle(PainelEducacionalIdebQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPainelEducacionalIdeb.ObterIdeb();
        }
    }
}
