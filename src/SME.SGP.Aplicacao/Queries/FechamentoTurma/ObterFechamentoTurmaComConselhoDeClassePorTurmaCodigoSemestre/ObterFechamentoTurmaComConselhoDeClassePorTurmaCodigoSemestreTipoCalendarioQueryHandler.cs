using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQueryHandler : IRequestHandler<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery, FechamentoTurma>
    {
        private readonly IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurmaConsulta;

        public ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQueryHandler(IRepositorioFechamentoTurmaConsulta repositorioFechamentoTurmaConsulta)
        {
            this.repositorioFechamentoTurmaConsulta = repositorioFechamentoTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioFechamentoTurmaConsulta));
        }

        public async Task<FechamentoTurma> Handle(ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreTipoCalendarioQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFechamentoTurmaConsulta
                .ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestre(request.CodigoTurma, request.Bimestre, request.AnoLetivoTurma, request.Semestre,request.TipoCalendario);
        }
    }
}