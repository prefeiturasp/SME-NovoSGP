using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFotoSemestreAlunoPorCodigoQueryHandler : IRequestHandler<ObterFotoSemestreAlunoPorCodigoQuery, AcompanhamentoAlunoFoto>
    {
        private readonly IRepositorioAcompanhamentoAlunoFoto repositorio;

        public ObterFotoSemestreAlunoPorCodigoQueryHandler(IRepositorioAcompanhamentoAlunoFoto repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<AcompanhamentoAlunoFoto> Handle(ObterFotoSemestreAlunoPorCodigoQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterFotoPorCodigo(request.CodigoFoto);
    }
}
