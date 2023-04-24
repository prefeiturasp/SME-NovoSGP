using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.HistoricoEscolar
{
    public class SalvarHistoricoEscolarObservacaoCommandHandler : IRequestHandler<SalvarHistoricoEscolarObservacaoCommand, long>
    {
        private readonly IRepositorioHistoricoEscolarObservacao repositorioHistoricoEscolarObservacao;

        public SalvarHistoricoEscolarObservacaoCommandHandler(IRepositorioHistoricoEscolarObservacao repositorioHistoricoEscolarObservacao)
        {
            this.repositorioHistoricoEscolarObservacao = repositorioHistoricoEscolarObservacao ?? throw new ArgumentNullException(nameof(repositorioHistoricoEscolarObservacao));
        }

        public Task<long> Handle(SalvarHistoricoEscolarObservacaoCommand request, CancellationToken cancellationToken)
        {
            return repositorioHistoricoEscolarObservacao.SalvarAsync(request.HistoricoEscolarObservacao);
        }
    }
}
