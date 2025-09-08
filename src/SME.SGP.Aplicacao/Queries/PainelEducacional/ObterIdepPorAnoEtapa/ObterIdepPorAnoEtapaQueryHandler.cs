using MediatR;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdepPorAnoEtapa
{
    public class ObterIdepPorAnoEtapaQueryHandler : IRequestHandler<ObterIdepPorAnoEtapaQuery, IEnumerable<PainelEducacionalConsolidacaoIdep>>
    {
        private readonly IRepositorioIdepPainelEducacionalConsulta repositorioIdepConsulta;
        public ObterIdepPorAnoEtapaQueryHandler(IRepositorioIdepPainelEducacionalConsulta repositorioIdepConsulta)
        {
            this.repositorioIdepConsulta = repositorioIdepConsulta ?? throw new ArgumentNullException(nameof(repositorioIdepConsulta));
        }
        public async Task<IEnumerable<PainelEducacionalConsolidacaoIdep>> Handle(ObterIdepPorAnoEtapaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioIdepConsulta.ObterIdepPorAnoEtapa(request.Ano, request.Etapa);
        }
    }
}
