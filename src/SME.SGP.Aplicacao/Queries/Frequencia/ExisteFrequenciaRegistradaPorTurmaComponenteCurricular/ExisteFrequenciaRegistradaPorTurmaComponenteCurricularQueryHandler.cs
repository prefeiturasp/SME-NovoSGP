﻿using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQueryHandler : IRequestHandler<ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery, bool>
    {
        private readonly IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo;

        public ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQueryHandler(IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta repositorioFrequenciaAlunoDisciplinaPeriodo)
        {
            this.repositorioFrequenciaAlunoDisciplinaPeriodo = repositorioFrequenciaAlunoDisciplinaPeriodo ?? throw new ArgumentNullException(nameof(repositorioFrequenciaAlunoDisciplinaPeriodo));
        }
        public async Task<bool> Handle(ExisteFrequenciaRegistradaPorTurmaComponenteCurricularQuery request, CancellationToken cancellationToken)
        {
            return await repositorioFrequenciaAlunoDisciplinaPeriodo.ExisteFrequenciaRegistradaPorTurmaComponenteCurricular(request.CodigoTurma, request.ComponentesCurricularesId, request.PeriodoEscolarId);
        }
    }
}
