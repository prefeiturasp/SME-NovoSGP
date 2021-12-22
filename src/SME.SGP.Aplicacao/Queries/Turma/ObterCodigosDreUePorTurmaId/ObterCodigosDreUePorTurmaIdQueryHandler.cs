﻿using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosDreUePorTurmaIdQueryHandler : IRequestHandler<ObterCodigosDreUePorTurmaIdQuery, DreUeDaTurmaDto>
    {
        private readonly IRepositorioTurmaConsulta repositorioTurma;

        public ObterCodigosDreUePorTurmaIdQueryHandler(IRepositorioTurmaConsulta repositorioTurma)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
        }

        public async Task<DreUeDaTurmaDto> Handle(ObterCodigosDreUePorTurmaIdQuery request, CancellationToken cancellationToken)
            => await repositorioTurma.ObterCodigosDreUePorId(request.TurmaId);
    }
}
