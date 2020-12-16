using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterBimestrePorTurmaCodigoQueryHandler : IRequestHandler<ObterBimestrePorTurmaCodigoQuery, int>
    {
        private readonly IRepositorioTurma repositorioTurma;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;

        public ObterBimestrePorTurmaCodigoQueryHandler(IRepositorioTurma repositorioTurma, IConsultasPeriodoEscolar consultasPeriodoEscolar)
        {
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
        }
        public async Task<int> Handle(ObterBimestrePorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            var turma = await repositorioTurma.ObterPorCodigo(request.TurmaCodigo);
            return await consultasPeriodoEscolar.ObterBimestre(request.Data, turma.ModalidadeCodigo, turma.Semestre);
        }
    }
}
