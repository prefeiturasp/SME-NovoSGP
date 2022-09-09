﻿using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIndicativoPendenciasAulasPorTipoQueryHandler : IRequestHandler<ObterIndicativoPendenciasAulasPorTipoQuery, PendenciaPaginaInicialListao>
    {
        private readonly IRepositorioPendenciaAulaConsulta repositorioPendenciaAula;
        private readonly IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordo;

        public ObterIndicativoPendenciasAulasPorTipoQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula, IRepositorioPendenciaDiarioBordoConsulta repositorioPendenciaDiarioBordo)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
            this.repositorioPendenciaDiarioBordo = repositorioPendenciaDiarioBordo ?? throw new ArgumentNullException(nameof(repositorioPendenciaDiarioBordo));
        }

        public async Task<PendenciaPaginaInicialListao> Handle(ObterIndicativoPendenciasAulasPorTipoQuery request, CancellationToken cancellationToken)
        {
            var aulasComPendenciaDiario = await repositorioPendenciaDiarioBordo.TrazerAulasComPendenciasDiarioBordo(request.DisciplinaId, request.ProfessorRf, request.EhGestor, request.TurmaId);
            var pendenciasDiarioBordo = await repositorioPendenciaDiarioBordo.TurmasPendenciaDiarioBordo(aulasComPendenciaDiario, request.TurmaId, request.Bimestre);
            if (request.ProfessorNaoCj)
                pendenciasDiarioBordo = pendenciasDiarioBordo.Where(p => !p.AulaCJ);


            bool validaSeTemPendenciaParaTurma = pendenciasDiarioBordo.Any(p => p.TurmaId == request.TurmaId);

            var temPendenciaDiarioBordo = request.VerificaDiarioBordo && validaSeTemPendenciaParaTurma;

            var temPendenciaAvaliacao = request.VerificaAvaliacao &&
                await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.Avaliacao, request.Bimestre, request.ProfessorCj, request.ProfessorNaoCj, request.ProfessorRf);

            var temPendenciaFrequencia = request.VerificaFrequencia &&
                await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.Frequencia, request.Bimestre, request.ProfessorCj, request.ProfessorNaoCj, request.ProfessorRf);

            var temPendenciaPlanoAula = request.VerificaPlanoAula &&
                await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.PlanoAula, request.Bimestre, request.ProfessorCj, request.ProfessorNaoCj, request.ProfessorRf);


            return new PendenciaPaginaInicialListao
            {
                PendenciaDiarioBordo = temPendenciaDiarioBordo,
                PendenciaAvaliacoes = temPendenciaAvaliacao,
                PendenciaFrequencia = temPendenciaFrequencia,
                PendenciaPlanoAula = temPendenciaPlanoAula
            };


        }
    }
}
