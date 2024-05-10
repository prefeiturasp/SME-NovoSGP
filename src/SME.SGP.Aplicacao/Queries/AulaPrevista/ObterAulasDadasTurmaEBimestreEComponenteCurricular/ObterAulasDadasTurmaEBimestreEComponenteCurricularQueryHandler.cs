﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasDadasTurmaEBimestreEComponenteCurricularQueryHandler : IRequestHandler<ObterAulasDadasTurmaEBimestreEComponenteCurricularQuery, IEnumerable<TurmaComponenteQntAulasDto>>
    {
        private readonly IRepositorioAulaPrevistaBimestreConsulta repositorioAulaPrevistaBimestre;        

        public ObterAulasDadasTurmaEBimestreEComponenteCurricularQueryHandler(IRepositorioAulaPrevistaBimestreConsulta repositorioAulaPrevistaBimestre)
        {
            this.repositorioAulaPrevistaBimestre = repositorioAulaPrevistaBimestre;            
        }

        public async Task<IEnumerable<TurmaComponenteQntAulasDto>> Handle(ObterAulasDadasTurmaEBimestreEComponenteCurricularQuery request, CancellationToken cancellationToken)
        {

            var aulaPrevistaAgrupada = await repositorioAulaPrevistaBimestre.ObterBimestresAulasTurmasComponentesCumpridasAsync(request.TurmasCodigo, request.ComponentesCurricularesId,
                request.TipoCalendarioId, request.Bimestres);

            if (aulaPrevistaAgrupada.NaoEhNulo())
            {

                var lista = aulaPrevistaAgrupada.Select(a =>

                    new TurmaComponenteQntAulasDto()
                    {
                        AulasQuantidade = a.AulasQuantidade,
                        ComponenteCurricularCodigo = a.ComponenteCurricularCodigo,
                        TurmaCodigo = a.TurmaCodigo,
                        Bimestre = a.Bimestre,
                        PeriodoEscolarId = a.PeriodoEscolarId
                    }
                );

                return lista;

            }

            return default;
        }
    }
}