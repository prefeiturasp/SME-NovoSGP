﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesRegenciaPorTurmaCodigoQueryHandler : IRequestHandler<ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        public ObterComponentesCurricularesRegenciaPorTurmaCodigoQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular, IMediator mediator)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesRegenciaPorTurmaCodigoQuery request, CancellationToken cancellationToken)
        {
            var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo));

            if (turma == null)
            {
                if (request.SituacaoConselho)
                    return default;
                else
                    throw new NegocioException("Turma não encontrada.");
            }                

            var ehQtdeDuracaoAulaTurma4h = turma.QuantidadeDuracaoAula == 4;
            var turno = ehQtdeDuracaoAulaTurma4h ? turma.QuantidadeDuracaoAula : 0;
            var ano = ehQtdeDuracaoAulaTurma4h ? Convert.ToInt64(turma.Ano.ToUpper().Replace('S', '1')) : 0;

            var regencias = await repositorioComponenteCurricular.ObterComponentesCurricularesRegenciaPorAnoETurno(ano, turno);

            return regencias.OrderBy(c => c.Nome);
        }
    }
}
