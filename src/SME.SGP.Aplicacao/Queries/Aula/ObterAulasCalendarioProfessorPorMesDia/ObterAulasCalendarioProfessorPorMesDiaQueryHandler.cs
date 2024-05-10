﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasCalendarioProfessorPorMesDiaQueryHandler : IRequestHandler<ObterAulasCalendarioProfessorPorMesDiaQuery, IEnumerable<Aula>>
    {
        private readonly IRepositorioAulaConsulta repositorioAula;

        public ObterAulasCalendarioProfessorPorMesDiaQueryHandler(IRepositorioAulaConsulta repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }
        public async Task<IEnumerable<Aula>> Handle(ObterAulasCalendarioProfessorPorMesDiaQuery request, CancellationToken cancellationToken)
        {
            return await repositorioAula.ObterAulasProfessorCalendarioPorData(request.TurmaCodigo, request.UeCodigo,  request.DiaConsulta); 
        }
    }
}
