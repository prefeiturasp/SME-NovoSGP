using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAlunosSemFrequenciaPorTurmaBimestreQueryHandler : IRequestHandler<ObterTotalAlunosSemFrequenciaPorTurmaBimestreQuery, IEnumerable<int>>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasse;
        public ObterTotalAlunosSemFrequenciaPorTurmaBimestreQueryHandler(IRepositorioConselhoClasseConsulta repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse;
        }
        public async Task<IEnumerable<int>> Handle(ObterTotalAlunosSemFrequenciaPorTurmaBimestreQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasse.ObterTotalAulasSemFrequenciaPorTurmaBimestre(request.DisciplinaId, request.CodigoTurma, request.Bimestre, request.DataMatricula);
        }
    }
}
