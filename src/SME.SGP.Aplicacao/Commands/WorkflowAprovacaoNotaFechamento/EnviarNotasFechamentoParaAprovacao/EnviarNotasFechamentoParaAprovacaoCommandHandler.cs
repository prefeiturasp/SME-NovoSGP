﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotasFechamentoParaAprovacaoCommandHandler : AsyncRequestHandler<EnviarNotasFechamentoParaAprovacaoCommand>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento;

        public EnviarNotasFechamentoParaAprovacaoCommandHandler(IMediator mediator,
                                                                IRepositorioWfAprovacaoNotaFechamento repositorioWfAprovacaoNotaFechamento)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioWfAprovacaoNotaFechamento = repositorioWfAprovacaoNotaFechamento ?? throw new ArgumentNullException(nameof(repositorioWfAprovacaoNotaFechamento));
        }

        protected override async Task Handle(EnviarNotasFechamentoParaAprovacaoCommand request, CancellationToken cancellationToken)
        {
            foreach (var notaFechamento in request.NotasAprovacao)
            {
                if(notaFechamento.Id > 0)
                {
                    await mediator.Send(new ExcluirWFAprovacaoNotaFechamentoPorNotaCommand(notaFechamento.Id));

                    await repositorioWfAprovacaoNotaFechamento.SalvarAsync(new WfAprovacaoNotaFechamento()
                    {
                        FechamentoNotaId = notaFechamento.Id,
                        Nota = notaFechamento.Nota,
                        ConceitoId = notaFechamento.ConceitoId,
                        CriadoEm = DateTimeExtension.HorarioBrasilia(),
                        CriadoPor = request.Usuario.Nome,
                        CriadoRF = request.Usuario.CodigoRf,
                    });
                }
                   
            }
        } 
    }
}
