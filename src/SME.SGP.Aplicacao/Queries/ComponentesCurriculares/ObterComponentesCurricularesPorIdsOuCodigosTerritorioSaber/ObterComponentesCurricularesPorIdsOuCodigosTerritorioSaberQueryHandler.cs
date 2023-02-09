﻿using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsOuCodigosTerritorioSaberQueryHandler : IRequestHandler<ObterComponentesCurricularesPorIdsOuCodigosTerritorioSaberQuery, IEnumerable<DisciplinaDto>>
    {
        private readonly IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular;
        private readonly IServicoEol servicoEol;
        private readonly IMediator mediator;

        public ObterComponentesCurricularesPorIdsOuCodigosTerritorioSaberQueryHandler(IRepositorioComponenteCurricularConsulta repositorioComponenteCurricular, IServicoEol servicoEol, IMediator mediator)
        {
            this.repositorioComponenteCurricular = repositorioComponenteCurricular ?? throw new ArgumentNullException(nameof(repositorioComponenteCurricular));
            this.servicoEol = servicoEol ?? throw new ArgumentNullException(nameof(servicoEol));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<DisciplinaDto>> Handle(ObterComponentesCurricularesPorIdsOuCodigosTerritorioSaberQuery request, CancellationToken cancellationToken)
        {
            if (request.PossuiTerritorio.HasValue && request.PossuiTerritorio.Value)
            {
                var listaDisciplinas = new List<DisciplinaDto>();
                var disciplinasAgrupadas = await servicoEol.ObterDisciplinasPorIdsAgrupadas(request.CodigoComponentes.Select(c => c.codigo).ToArray(), request.CodigoTurma);
                foreach (var disciplina in disciplinasAgrupadas)
                {
                    var codigoTerritorioSaberCorrespondente = request.CodigoComponentes.Single(c => c.codigo.Equals(disciplina.CodigoComponenteCurricular));
                    disciplina.RegistraFrequencia = await mediator.Send(new ObterComponenteRegistraFrequenciaQuery(disciplina.CodigoComponenteCurricular, codigoTerritorioSaberCorrespondente.codigoTerritorioSaber));
                    disciplina.Id = codigoTerritorioSaberCorrespondente.codigoTerritorioSaber ?? 0;
                    listaDisciplinas.Add(disciplina);
                }

                return listaDisciplinas;
            }
            else
                return await repositorioComponenteCurricular.ObterDisciplinasPorIds(request.CodigoComponentes.Select(c => c.codigo).ToArray());
        }
    }
}
