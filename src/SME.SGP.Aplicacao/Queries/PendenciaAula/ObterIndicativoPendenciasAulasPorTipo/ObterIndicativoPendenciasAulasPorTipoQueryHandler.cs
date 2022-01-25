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
            var temPendenciaDiarioBordo = await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.DiarioBordo, "diario_bordo", new long[] { (int)Modalidade.EducacaoInfantil }, request.AnoLetivo);

            var temPendenciaAvaliacao = await repositorioPendenciaAula.PossuiPendenciasAtividadeAvaliativa(request.DisciplinaId, request.TurmaId, request.AnoLetivo);

            var temPendenciaFrequencia = await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.Frequencia, "registro_frequencia", new long[] { (int)Modalidade.EducacaoInfantil, (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio }, request.AnoLetivo);

            var temPendenciaPlanoAula =  await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.PlanoAula, "plano_aula", new long[] { (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio }, request.AnoLetivo);


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
