using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarAcompanhamentoTurmaCommandHandler : IRequestHandler<SalvarAcompanhamentoTurmaCommand, AcompanhamentoTurma>
    {
        private readonly IRepositorioAcompanhamentoTurma repositorio;

        public SalvarAcompanhamentoTurmaCommandHandler(IRepositorioAcompanhamentoTurma repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<AcompanhamentoTurma> Handle(SalvarAcompanhamentoTurmaCommand request, CancellationToken cancellationToken)
        {
            await repositorio.SalvarAsync(request.Acompanhamento);

            return request.Acompanhamento;
        }
    }
}
