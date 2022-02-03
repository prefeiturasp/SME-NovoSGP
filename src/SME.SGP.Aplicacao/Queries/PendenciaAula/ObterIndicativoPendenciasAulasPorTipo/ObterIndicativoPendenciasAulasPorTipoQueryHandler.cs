using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
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
            var temPendenciaDiarioBordo = request.VerificaDiarioBordo &&
                await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.DiarioBordo, request.Bimestre,request.ProfessorCj,request.ProfessorNaoCj,request.ProfessorRf);

            var temPendenciaAvaliacao = request.VerificaAvaliacao &&
                await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.Avaliacao, request.Bimestre, request.ProfessorCj, request.ProfessorNaoCj,request.ProfessorRf);

            var temPendenciaFrequencia = request.VerificaFrequencia &&
                await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.Frequencia, request.Bimestre, request.ProfessorCj, request.ProfessorNaoCj,request.ProfessorRf);

            var temPendenciaPlanoAula = request.VerificaPlanoAula &&
                await repositorioPendenciaAula.PossuiPendenciasPorTipo(request.DisciplinaId, request.TurmaId, TipoPendencia.PlanoAula, request.Bimestre, request.ProfessorCj, request.ProfessorNaoCj,request.ProfessorRf);


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
