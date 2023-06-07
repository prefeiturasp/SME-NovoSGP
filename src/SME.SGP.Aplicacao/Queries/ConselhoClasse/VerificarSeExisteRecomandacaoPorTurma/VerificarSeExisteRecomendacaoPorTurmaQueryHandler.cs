using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class VerificarSeExisteRecomendacaoPorTurmaQueryHandler : IRequestHandler<VerificarSeExisteRecomendacaoPorTurmaQuery,IEnumerable<AlunoTemRecomandacaoDto>>
    {
        private readonly IRepositorioConselhoClasse repoConselhoClasse;

        public VerificarSeExisteRecomendacaoPorTurmaQueryHandler(IRepositorioConselhoClasse repositorioConselhoClasse)
        {
            this.repoConselhoClasse = repositorioConselhoClasse ?? throw new ArgumentNullException(nameof(repositorioConselhoClasse));
        }

        public async Task<IEnumerable<AlunoTemRecomandacaoDto>> Handle(VerificarSeExisteRecomendacaoPorTurmaQuery request, CancellationToken cancellationToken)
        {
            return await repoConselhoClasse.VerificarSeExisteRecomendacaoPorTurma(request.TurmasCodigo, request.Bimestre);
        }
    }
}