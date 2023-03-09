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
        private readonly IMediator mediator;

        public ObterAtribuicaoResponsaveisPorUeTipoQueryHandler(IRepositorioSupervisorEscolaDre repositorioSupervisorEscolaDre, IMediator mediator)
        {
            this.repositorioSupervisorEscolaDre = repositorioSupervisorEscolaDre ?? throw new ArgumentNullException(nameof(repositorioSupervisorEscolaDre));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<AtribuicaoResponsavelDto>> Handle(ObterAtribuicaoResponsaveisPorUeTipoQuery request, CancellationToken cancellationToken)
        {
            var supervisores = await repositorioSupervisorEscolaDre.ObterSupervisoresPorUeTipo(request.CodigoUE, request.Tipo);

            var retorno = supervisores.Select(a => new AtribuicaoResponsavelDto()
            {
                CodigoRF = a.SupervisorId,
                NomeResponsavel = a.NomeResponsavel
            }).ToList();

            var supervisoresSemNome = retorno.Where(supervisor => string.IsNullOrEmpty(supervisor.NomeResponsavel));
            if (supervisoresSemNome.Any())
            {
                var funcionarios = await mediator.Send(new ObterFuncionariosPorRFsQuery(supervisoresSemNome.Select(supervisor => supervisor.CodigoRF)));
                foreach(var funcionario in funcionarios)
                {
                    foreach (var supervisor in retorno.Where(supervisor => supervisor.CodigoRF == funcionario.CodigoRF))
                        supervisor.NomeResponsavel = funcionario.Nome;
                }
            }

            return retorno;
        }
    }
}