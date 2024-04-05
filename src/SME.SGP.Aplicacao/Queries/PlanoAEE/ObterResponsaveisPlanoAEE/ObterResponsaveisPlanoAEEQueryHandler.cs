using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisPlanoAEEQueryHandler : IRequestHandler<ObterResponsaveisPlanoAEEQuery, IEnumerable<UsuarioEolRetornoDto>>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEE;

        public ObterResponsaveisPlanoAEEQueryHandler(IRepositorioPlanoAEEConsulta repositorioPlanoAEE) 
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public Task<IEnumerable<UsuarioEolRetornoDto>> Handle(ObterResponsaveisPlanoAEEQuery request, CancellationToken cancellationToken)
        {
            return repositorioPlanoAEE.ObterResponsaveis(request.DreId, new long[] { request.UeId }, request.TurmaId, request.AlunoCodigo, (int?)request.Situacao, request.ExibirEncerrados);
        }
    }
}
