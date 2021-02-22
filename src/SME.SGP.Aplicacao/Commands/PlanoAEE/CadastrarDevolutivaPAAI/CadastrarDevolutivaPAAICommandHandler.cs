﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands
{
    public class CadastrarDevolutivaPAAICommandHandler : IRequestHandler<CadastrarDevolutivaPAAICommand, bool>
    {

        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IMediator mediator;

        public CadastrarDevolutivaPAAICommandHandler(
            IMediator mediator,
            IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

       
        public async Task<bool> Handle(CadastrarDevolutivaPAAICommand request, CancellationToken cancellationToken)
        {
            var planoAEE = await repositorioPlanoAEE.ObterPorIdAsync(request.PlanoAEEId);

            if (planoAEE == null)
                throw new NegocioException("Plano AEE não encontrado!");

            var usuarioLogado = await mediator.Send(new ObterUsuarioLogadoIdQuery());

            if (planoAEE.ResponsavelId != usuarioLogado)
                throw new NegocioException("O usuário atual não é o PAAI responsável por este Plano AEE");

            planoAEE.Situacao = Dominio.Enumerados.SituacaoPlanoAEE.Encerrado;
            planoAEE.ParecerPAAI = request.ParecerPAAI;

            var idEntidadeEncaminhamento = await repositorioPlanoAEE.SalvarAsync(planoAEE);

            return idEntidadeEncaminhamento != 0;
        }
    }
}
