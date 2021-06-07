﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPendenciasAtividadeAvaliativaQueryHandler : IRequestHandler<ObterPendenciasAtividadeAvaliativaQuery, IEnumerable<Aula>>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;

        public ObterPendenciasAtividadeAvaliativaQueryHandler(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<IEnumerable<Aula>> Handle(ObterPendenciasAtividadeAvaliativaQuery request, CancellationToken cancellationToken)
            => await repositorioPendenciaAula.ListarPendenciasAtividadeAvaliativa(request.AnoLetivo);
    }
}
