using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteConselhoClasseParaTurmaQueryHandler : IRequestHandler<ExisteConselhoClasseParaTurmaQuery, bool>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorio;

        public ExisteConselhoClasseParaTurmaQueryHandler(IRepositorioConselhoClasseConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<bool> Handle(ExisteConselhoClasseParaTurmaQuery request, CancellationToken cancellationToken)
        {
            return this.repositorio.ExisteConselhoDeClasseParaTurma(request.CodigosTurmas, request.Bimestre);
        }
    }
}
