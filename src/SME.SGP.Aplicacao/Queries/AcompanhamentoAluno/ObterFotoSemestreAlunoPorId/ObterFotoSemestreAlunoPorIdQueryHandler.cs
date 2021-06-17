using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterFotoSemestreAlunoPorIdQueryHandler : IRequestHandler<ObterFotoSemestreAlunoPorIdQuery, AcompanhamentoAlunoFoto>
    {
        private readonly IRepositorioAcompanhamentoAlunoFoto repositorio;

        public ObterFotoSemestreAlunoPorIdQueryHandler(IRepositorioAcompanhamentoAlunoFoto repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<AcompanhamentoAlunoFoto> Handle(ObterFotoSemestreAlunoPorIdQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterPorIdAsync(request.AcompanhamentoAlunoFotoId);
    }
}
