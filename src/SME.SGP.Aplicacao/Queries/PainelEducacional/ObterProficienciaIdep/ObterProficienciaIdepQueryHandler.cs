using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdep
{
    public class ObterProficienciaIdepQueryHandler : IRequestHandler<ObterProficienciaIdepQuery, IEnumerable<ProficienciaIdepAgrupadaDto>>
    {
        private readonly IRepositorioPainelEducacionalProficienciaIdep repositorioProficienciaIdeb;
        public ObterProficienciaIdepQueryHandler(IRepositorioPainelEducacionalProficienciaIdep repositorioIdebConsulta)
        {
            this.repositorioProficienciaIdeb = repositorioIdebConsulta ?? throw new ArgumentNullException(nameof(repositorioIdebConsulta));
        }

        public async Task<IEnumerable<ProficienciaIdepAgrupadaDto>> Handle(ObterProficienciaIdepQuery request, CancellationToken cancellationToken)
        {
            return await repositorioProficienciaIdeb.ObterProficienciaIdep(request.AnoLetivo, request.CodigoUe);
        }
    }
}
