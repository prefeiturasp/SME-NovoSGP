using MediatR;
using SME.SGP.Dominio.Interfaces.Repositorios;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirTodasTurmasComunicadoCommandHandler : IRequestHandler<ExcluirTodasTurmasComunicadoCommand, bool>
    {
        private readonly IRepositorioComunicadoTurma repositorioComunicadoTurma;

        public ExcluirTodasTurmasComunicadoCommandHandler(IRepositorioComunicadoTurma repositorioComunicadoTurma)
        {
            this.repositorioComunicadoTurma = repositorioComunicadoTurma ?? throw new ArgumentNullException(nameof(repositorioComunicadoTurma));
        }

        public async Task<bool> Handle(ExcluirTodasTurmasComunicadoCommand request, CancellationToken cancellationToken)
            => await repositorioComunicadoTurma.RemoverTodasTurmasComunicado(request.Id);        
    }
}
