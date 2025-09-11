using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdep
{
    public class ObterIdepQueryHandler : IRequestHandler<ObterIdepQuery, IEnumerable<PainelEducacionalConsolidacaoIdep>>
    {
        private readonly IRepositorioIdepPainelEducacionalConsulta repositorioIdepConsulta;

        public ObterIdepQueryHandler(IRepositorioIdepPainelEducacionalConsulta repositorioIdepConsulta)
        {
            this.repositorioIdepConsulta = repositorioIdepConsulta ?? throw new ArgumentNullException(nameof(repositorioIdepConsulta));
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoIdep>> Handle(ObterIdepQuery request, CancellationToken cancellationToken)
        {
            return await repositorioIdepConsulta.ObterTodosIdep();
        }
    }
}