using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecoesMapeamentoEstudantePorMapeamentoIdCommandHandler : IRequestHandler<ExcluirSecoesMapeamentoEstudantePorMapeamentoIdCommand, bool>
    {
        public IRepositorioMapeamentoEstudanteSecao repositorioMapeamentoSecao { get; }
        public IMediator mediator { get; }

        public ExcluirSecoesMapeamentoEstudantePorMapeamentoIdCommandHandler(IMediator mediator, IRepositorioMapeamentoEstudanteSecao repositorioMapeamentoSecao)
        {
            this.repositorioMapeamentoSecao = repositorioMapeamentoSecao ?? throw new ArgumentNullException(nameof(repositorioMapeamentoSecao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirSecoesMapeamentoEstudantePorMapeamentoIdCommand request, CancellationToken cancellationToken)
        {
            var secoesIds = await repositorioMapeamentoSecao.ObterIdsSecoesPorMapeamentoEstudanteId(request.MapeamentoEstudanteId);
            foreach(var secaoId in secoesIds)
                await mediator.Send(new ExcluirMapeamentoEstudantePorSecaoIdCommand(secaoId));          
            await repositorioMapeamentoSecao.RemoverLogico(request.MapeamentoEstudanteId, "mapeamento_estudante_id");
            return true;
        }
    }
}
