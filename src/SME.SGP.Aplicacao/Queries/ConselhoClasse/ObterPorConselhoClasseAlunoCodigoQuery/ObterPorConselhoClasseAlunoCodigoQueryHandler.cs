using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPorConselhoClasseAlunoCodigoQueryHandler : IRequestHandler<ObterPorConselhoClasseAlunoCodigoQuery, ConselhoClasseAluno>
    {
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorio;

        public ObterPorConselhoClasseAlunoCodigoQueryHandler(IRepositorioConselhoClasseAlunoConsulta repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<ConselhoClasseAluno> Handle(ObterPorConselhoClasseAlunoCodigoQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterPorConselhoClasseAlunoCodigoAsync(request.ConselhoClasseId, request.AlunoCodigo);
        }        
    }
}
