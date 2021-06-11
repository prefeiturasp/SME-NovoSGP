using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFotoAlunoPorAlunoCodigoQueryHandler : IRequestHandler<ObterFotoAlunoPorAlunoCodigoQuery, AlunoFoto>
    {
        private readonly IRepositorioAlunoFoto repositorio;

        public ObterFotoAlunoPorAlunoCodigoQueryHandler(IRepositorioAlunoFoto repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<AlunoFoto> Handle(ObterFotoAlunoPorAlunoCodigoQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterFotoPorAlunoCodigo(request.CodigoAluno);
    }
}
