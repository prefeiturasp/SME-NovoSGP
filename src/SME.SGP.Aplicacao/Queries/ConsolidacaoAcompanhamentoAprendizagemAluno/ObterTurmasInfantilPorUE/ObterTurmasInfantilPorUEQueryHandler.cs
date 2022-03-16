using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasInfantilPorUEQueryHandler : IRequestHandler<ObterTurmasInfantilPorUEQuery, IEnumerable<TurmaDTO>>
    {
        private readonly IRepositorioTurmaConsulta repositorio;

        public ObterTurmasInfantilPorUEQueryHandler(IRepositorioTurmaConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<TurmaDTO>> Handle(ObterTurmasInfantilPorUEQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterTurmasInfantilPorAno(request.AnoLetivo, request.UeCodigo);
    }
}
