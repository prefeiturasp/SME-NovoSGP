using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQueryHandler : IRequestHandler<ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQuery, ConselhoClasseAluno>
    {
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta;

        public ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQueryHandler(IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta)
        {
            this.repositorioConselhoClasseAlunoConsulta = repositorioConselhoClasseAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAlunoConsulta));
        }

        public async Task<ConselhoClasseAluno> Handle(ObterPorConselhoClasseAlunoPorTurmaAlunoBimestreQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseAlunoConsulta.ObterPorFiltrosAsync(request.CodigoTurma, request.CodigoAluno, request.Bimestre, request.EhFinal);
        }        
    }
}
