using MediatR;
using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEEncerradosAutomaticamenteQueryHandler : IRequestHandler<ObterPlanosAEEEncerradosAutomaticamenteQuery, IEnumerable<PlanoAEE>>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEEConsulta;

        public ObterPlanosAEEEncerradosAutomaticamenteQueryHandler(IRepositorioPlanoAEEConsulta repositorioPlanoAEEConsulta)
        {
            this.repositorioPlanoAEEConsulta = repositorioPlanoAEEConsulta ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEConsulta));
        }

        public async Task<IEnumerable<PlanoAEE>> Handle(ObterPlanosAEEEncerradosAutomaticamenteQuery request, CancellationToken cancellationToken) =>
            await repositorioPlanoAEEConsulta.ObterPlanosEncerradosAutomaticamente(request.Pagina, request.QuantidadeRegistrosPorPagina);
    }
}
