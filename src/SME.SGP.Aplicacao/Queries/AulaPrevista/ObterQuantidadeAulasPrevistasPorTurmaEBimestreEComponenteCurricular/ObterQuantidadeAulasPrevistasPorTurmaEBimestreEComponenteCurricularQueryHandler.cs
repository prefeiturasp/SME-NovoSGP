﻿using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQueryHandler : IRequestHandler<ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery, int>
    {
        private readonly IRepositorioAulaPrevistaBimestreConsulta repositorioAulaPrevistaBimestre;

        public ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQueryHandler(IRepositorioAulaPrevistaBimestreConsulta repositorioAulaPrevistaBimestre)
        {
            this.repositorioAulaPrevistaBimestre = repositorioAulaPrevistaBimestre;
        }

        public async Task<int> Handle(ObterQuantidadeAulasPrevistasPorTurmaEBimestreEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            var aulaPrevistaBimestre = await repositorioAulaPrevistaBimestre.ObterAulasPrevistasPorTurmaTipoCalendarioDisciplina(request.TipoCalendarioId, request.CodigoTurma, request.ComponentesCurricularesId.Select(cc => cc.ToString()).ToArray(), request.Bimestre, request.Professor);
            return aulaPrevistaBimestre?.Sum(x => x.Previstas) ?? default;
        }
    }
}