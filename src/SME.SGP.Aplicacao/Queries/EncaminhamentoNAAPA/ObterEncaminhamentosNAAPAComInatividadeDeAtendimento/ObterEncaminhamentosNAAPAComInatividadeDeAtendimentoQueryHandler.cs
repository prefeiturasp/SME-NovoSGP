﻿using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQueryHandler : IRequestHandler<ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQuery, IEnumerable<EncaminhamentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>>
    {
        private readonly IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA;
        public ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQueryHandler(IRepositorioEncaminhamentoNAAPA repositorioEncaminhamentoNAAPA)
        {
            this.repositorioEncaminhamentoNAAPA = repositorioEncaminhamentoNAAPA ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPA));
        }

        public Task<IEnumerable<EncaminhamentoNAAPAInformacoesNotificacaoInatividadeAtendimentoDto>> Handle(ObterEncaminhamentosNAAPAComInatividadeDeAtendimentoQuery request, CancellationToken cancellationToken)
        {
            return this.repositorioEncaminhamentoNAAPA.ObterInformacoesDeNotificacaoDeInatividadeDeAtendimento(request.UeId);
        }
    }
}
