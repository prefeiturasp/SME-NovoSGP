using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterApanhadoGeralPorTurmaIdESemestreQueryHandler : IRequestHandler<ObterApanhadoGeralPorTurmaIdESemestreQuery, AcompanhamentoTurma>
    {
        private readonly IRepositorioAcompanhamentoTurma repositorioAcompanhamentoTurma;

        public ObterApanhadoGeralPorTurmaIdESemestreQueryHandler(IRepositorioAcompanhamentoTurma repositorioAcompanhamentoTurma)
        {
            this.repositorioAcompanhamentoTurma = repositorioAcompanhamentoTurma ?? throw new ArgumentNullException(nameof(repositorioAcompanhamentoTurma));
        }

        public async Task<AcompanhamentoTurma> Handle(ObterApanhadoGeralPorTurmaIdESemestreQuery request, CancellationToken cancellationToken)
            => await repositorioAcompanhamentoTurma.ObterApanhadoGeralPorTurmaIdESemestre(request.TurmaId, request.Semestre);
    }

}
