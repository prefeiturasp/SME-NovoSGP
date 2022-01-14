using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQueryHandler : IRequestHandler<ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQuery, IEnumerable<AusenciaMotivoDto>>
    {
        private readonly IRepositorioFrequenciaConsulta repositorioFrequencia;

        public ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQueryHandler(IRepositorioFrequenciaConsulta repositorioFrequencia)
        {
            this.repositorioFrequencia = repositorioFrequencia ?? throw new ArgumentNullException(nameof(repositorioFrequencia));
        }
        public async Task<IEnumerable<AusenciaMotivoDto>> Handle(ObterAusenciaMotivoPorAlunoTurmaBimestreAnoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFrequencia.ObterAusenciaMotivoPorAlunoTurmaBimestreAno(
                request.CodigoAluno,
                request.Turma,
                request.Bimestre,
                request.AnoLetivo
                );
        }
    }
}
