using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecoesRegistroAcaoPorRegistroAcaoIdCommandHandler : IRequestHandler<ExcluirSecoesRegistroAcaoPorRegistroAcaoIdCommand, bool>
    {
        public IRepositorioRegistroAcaoBuscaAtivaSecao repositorioRegistroAcaoSecao { get; }
        public IMediator mediator { get; }

        public ExcluirSecoesRegistroAcaoPorRegistroAcaoIdCommandHandler(IMediator mediator, IRepositorioRegistroAcaoBuscaAtivaSecao repositorioRegistroAcaoSecao)
        {
            this.repositorioRegistroAcaoSecao = repositorioRegistroAcaoSecao ?? throw new ArgumentNullException(nameof(repositorioRegistroAcaoSecao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirSecoesRegistroAcaoPorRegistroAcaoIdCommand request, CancellationToken cancellationToken)
        {
            var secoesIds = await repositorioRegistroAcaoSecao.ObterIdsSecoesPorRegistroAcaoId(request.RegistroAcaoId);
            foreach(var secaoId in secoesIds)
                await mediator.Send(new ExcluirRegistroAcaoPorSecaoIdCommand(secaoId));          
            await repositorioRegistroAcaoSecao.RemoverLogico(request.RegistroAcaoId, "registro_acao_busca_ativa_id");
            return true;
        }
    }
}
