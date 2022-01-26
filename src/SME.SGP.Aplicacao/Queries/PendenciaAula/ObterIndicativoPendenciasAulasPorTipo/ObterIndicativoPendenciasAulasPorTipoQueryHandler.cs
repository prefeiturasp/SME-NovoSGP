using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterIndicativoPendenciasAulasPorTipoQueryHandler : IRequestHandler<ObterIndicativoPendenciasAulasPorTipoQuery, PendenciaPaginaInicialListao>
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;

        public ObterIndicativoPendenciasAulasPorTipoQueryHandler(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<PendenciaPaginaInicialListao> Handle(ObterIndicativoPendenciasAulasPorTipoQuery request, CancellationToken cancellationToken)
        {
            var temPendenciaDiarioBordo = request.VerificaDiarioBordo &&
                await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.DiarioBordo, request.Bimestre);

            var temPendenciaAvaliacao = request.VerificaAvaliacao &&
                await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.Avaliacao, request.Bimestre);

            var temPendenciaFrequencia = request.VerificaFrequencia &&
                await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.Frequencia, request.Bimestre);

            var temPendenciaPlanoAula = request.VerificaPlanoAula &&
                await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.PlanoAula, request.Bimestre);


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
