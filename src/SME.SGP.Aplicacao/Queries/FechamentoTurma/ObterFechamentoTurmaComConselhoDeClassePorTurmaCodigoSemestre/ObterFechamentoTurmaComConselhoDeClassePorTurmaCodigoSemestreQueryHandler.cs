using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;

namespace SME.SGP.Aplicacao
{
    public class ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreQueryHandler : IRequestHandler<ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreQuery,FechamentoTurma>
    {
        private IRepositorioFechamentoTurmaConsulta _consulta;

        public ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreQueryHandler(IRepositorioFechamentoTurmaConsulta consulta)
        {
            _consulta = consulta ?? throw new ArgumentNullException(nameof(consulta));
        }

        public async Task<FechamentoTurma> Handle(ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestreQuery request, CancellationToken cancellationToken)
        {
            return await _consulta.ObterFechamentoTurmaComConselhoDeClassePorTurmaCodigoSemestre(request.CodigoTurma,request.Bimestre);
        }
    }
}