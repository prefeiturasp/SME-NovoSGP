﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoNAAPAComTurmaPorIdQueryHandler : IRequestHandler<ObterEncaminhamentoNAAPAComTurmaPorIdQuery, EncaminhamentoNAAPA>
    {
        public IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA { get; }

        public ObterEncaminhamentoNAAPAComTurmaPorIdQueryHandler(IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public async Task<EncaminhamentoNAAPA> Handle(ObterEncaminhamentoNAAPAComTurmaPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioEncaminhamentoNAAPA.ObterEncaminhamentoComTurmaPorId(request.EncaminhamentoId);
    }
}
