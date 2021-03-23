using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFotoSemestreAlunoPorMiniaturaIdQueryHandler : IRequestHandler<ObterFotoSemestreAlunoPorMiniaturaIdQuery, AcompanhamentoAlunoFoto>
    {
        private readonly IRepositorioAcompanhamentoAlunoFoto repositorio;

        public ObterFotoSemestreAlunoPorMiniaturaIdQueryHandler(IRepositorioAcompanhamentoAlunoFoto repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<AcompanhamentoAlunoFoto> Handle(ObterFotoSemestreAlunoPorMiniaturaIdQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterFotoPorMiniaturaId(request.MiniaturaId);
    }
}
