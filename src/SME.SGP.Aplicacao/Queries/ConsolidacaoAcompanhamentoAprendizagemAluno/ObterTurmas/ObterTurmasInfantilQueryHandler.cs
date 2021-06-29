using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasInfantilQueryHandler : IRequestHandler<ObterTurmasInfantilQuery, IEnumerable<TurmaDTO>>
    {
        private readonly IRepositorioTurma repositorio;

        public ObterTurmasInfantilQueryHandler(IRepositorioTurma repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<TurmaDTO>> Handle(ObterTurmasInfantilQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterTurmasInfantilPorAno(request.AnoLetivo);
        }
    }
}
