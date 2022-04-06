using MediatR;
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

        public ObterIndicativoPendenciasAulasPorTipoQueryHandler(IRepositorioPendenciaAulaConsulta repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        public async Task<PendenciaPaginaInicialListao> Handle(ObterIndicativoPendenciasAulasPorTipoQuery request, CancellationToken cancellationToken)
        {
            var pendenciasDiarioBordo = await repositorioPendenciaAula.PossuiPendenciaDiarioBordo(request.DisciplinaId, request.ProfessorCj, request.TurmaId, request.ProfessorRf);
            if (request.ProfessorNaoCj)
                pendenciasDiarioBordo = pendenciasDiarioBordo.Where(p => !p.AulaCJ);

            bool validaSeTemPendenciaDiarioBordo = pendenciasDiarioBordo.Any(p => p.TurmaId == request.TurmaId && p.Bimestre == request.Bimestre);

            var temPendenciaDiarioBordo = request.VerificaDiarioBordo && validaSeTemPendenciaDiarioBordo;

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
