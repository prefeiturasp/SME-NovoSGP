using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAcompanhamentoAlunoSemestrePorIdQueryHandler : IRequestHandler<ObterAcompanhamentoAlunoSemestrePorIdQuery, AcompanhamentoAlunoSemestre>
    {
        private readonly IRepositorioAcompanhamentoAlunoSemestre repositorioAcompanhamentoAlunoSemestre;

        public ObterAcompanhamentoAlunoSemestrePorIdQueryHandler(IRepositorioAcompanhamentoAlunoSemestre repositorioAcompanhamentoAlunoSemestre)
        {
            this.repositorioAcompanhamentoAlunoSemestre = repositorioAcompanhamentoAlunoSemestre ?? throw new ArgumentNullException(nameof(repositorioAcompanhamentoAlunoSemestre));
        }

        public async Task<AcompanhamentoAlunoSemestre> Handle(ObterAcompanhamentoAlunoSemestrePorIdQuery request, CancellationToken cancellationToken)
            => await repositorioAcompanhamentoAlunoSemestre.ObterPorIdAsync(request.Id);
    }
}
