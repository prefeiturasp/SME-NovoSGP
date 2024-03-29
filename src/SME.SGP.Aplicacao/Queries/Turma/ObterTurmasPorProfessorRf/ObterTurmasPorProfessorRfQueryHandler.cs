﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorProfessorRfQueryHandler : IRequestHandler<ObterTurmasPorProfessorRfQuery, IEnumerable<ProfessorTurmaDto>>
    {
        private readonly IMediator mediator;

        public ObterTurmasPorProfessorRfQueryHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<ProfessorTurmaDto>> Handle(ObterTurmasPorProfessorRfQuery request, CancellationToken cancellationToken)
        {
            return MapearParaDto(await mediator.Send(new ObterTurmasDoProfessorQuery(request.CodigoRf)));
        }

        private IEnumerable<ProfessorTurmaDto> MapearParaDto(IEnumerable<ProfessorTurmaReposta> turmas)
        {
            return turmas?.Select(m => new ProfessorTurmaDto()
            {
                Ano = m.Ano,
                AnoLetivo = m.AnoLetivo,
                CodDre = m.CodDre,
                CodEscola = m.CodEscola,
                CodModalidade = m.CodModalidade,
                CodTipoEscola = m.CodTipoEscola,
                CodTipoUE = m.CodTipoUE,
                CodTurma = m.CodTurma,
                Dre = m.Dre,
                DreAbrev = m.DreAbrev,
                Modalidade = m.Modalidade,
                NomeTurma = m.NomeTurma,
                TipoEscola = m.TipoEscola,
                Semestre = m.Semestre,
                TipoUE = m.TipoUE,
                Ue = m.Ue,
                UeAbrev = m.UeAbrev
            });
        }
    }
}
