using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterRelatorioPAPAlunoConselhoClasseQueryHandler : IRequestHandler<ObterRelatorioPAPAlunoConselhoClasseQuery, RelatorioPAPAlunoConselhoClasseDto>
    {
        private readonly IRepositorioRelatorioPeriodicoPAPAluno repositorio;

        public ObterRelatorioPAPAlunoConselhoClasseQueryHandler(IRepositorioRelatorioPeriodicoPAPAluno repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public Task<RelatorioPAPAlunoConselhoClasseDto> Handle(ObterRelatorioPAPAlunoConselhoClasseQuery request, CancellationToken cancellationToken)
        {
            return repositorio.ObterDadosRelatorioPAPAlunoConselhoClasse(request.AnoLetivo, request.AlunoCodigo, request.Bimestre, request.Modalidade);
        }
    }
}
