using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarParametroSistemaCommandHandler : IRequestHandler<AtualizarParametroSistemaCommand, long>
    {
        private readonly IRepositorioParametrosSistema repositorio;

        public AtualizarParametroSistemaCommandHandler(IRepositorioParametrosSistema repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<long> Handle(AtualizarParametroSistemaCommand request, CancellationToken cancellationToken)
            => await repositorio.SalvarAsync(request.Parametro);
    }
}
