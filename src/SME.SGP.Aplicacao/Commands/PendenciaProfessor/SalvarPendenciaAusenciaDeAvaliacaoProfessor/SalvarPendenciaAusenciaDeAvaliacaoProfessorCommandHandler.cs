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
    public class SalvarPendenciaAusenciaDeAvaliacaoProfessorCommandHandler : IRequestHandler<SalvarPendenciaAusenciaDeAvaliacaoProfessorCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;

        public SalvarPendenciaAusenciaDeAvaliacaoProfessorCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(SalvarPendenciaAusenciaDeAvaliacaoProfessorCommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(Dominio.TipoPendencia.AusenciaDeAvaliacaoProfessor, request.UeId, request.TurmaId, request.Mensagem, request.Instrucao, request.Titulo));
                    await mediator.Send(new SalvarPendenciaProfessorCommand(pendenciaId, request.TurmaId, request.ComponenteCurricularId, request.ProfessorRf, request.PeriodoEscolarId));
                    await GerarPendenciaUsuario(pendenciaId, request.ProfessorRf);

                    unitOfWork.PersistirTransacao();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
            return true;
        }

        private async Task GerarPendenciaUsuario(long pendenciaId, string professorRf)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(professorRf));
            await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));
        }
    }
}
