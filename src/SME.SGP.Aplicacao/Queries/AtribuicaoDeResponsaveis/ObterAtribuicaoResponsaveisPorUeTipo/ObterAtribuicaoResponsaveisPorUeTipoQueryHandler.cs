using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    internal class ObterAtribuicaoResponsaveisPorUeTipoQueryHandler : IRequestHandler<ObterAtribuicaoResponsaveisPorUeTipoQuery, IEnumerable<AtribuicaoResponsavelDto>>
    {
        private readonly IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre;

        public ObterAtribuicaoResponsaveisPorUeTipoQueryHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
        }
        public async Task<IEnumerable<AtribuicaoResponsavelDto>> Handle(ObterAtribuicaoResponsaveisPorUeTipoQuery request, CancellationToken cancellationToken)
        {
            var supervisores = await repositorioSupervisorEscolaDre.ObterSupervisoresPorUeTipo(request.CodigoUE, request.Tipo);

            return supervisores.Select(a => new AtribuicaoResponsavelDto()
            {
                CodigoRF = a.SupervisorId
            }).ToList();
        }
    }
}