using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAnoDoAcompanhamentoAlunoQueryHandler : IRequestHandler<ObterAnoDoAcompanhamentoAlunoQuery, int>
    {
        private readonly IRepositorioAcompanhamentoAlunoSemestre repositorio;

        public ObterAnoDoAcompanhamentoAlunoQueryHandler(IRepositorioAcompanhamentoAlunoSemestre repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<int> Handle(ObterAnoDoAcompanhamentoAlunoQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterAnoPorId(request.AcompanhamentoAlunoSemestreId);
    }
}
