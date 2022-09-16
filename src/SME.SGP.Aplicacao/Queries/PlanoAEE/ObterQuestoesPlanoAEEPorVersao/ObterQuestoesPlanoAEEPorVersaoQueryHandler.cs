using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesPlanoAEEPorVersaoQueryHandler : IRequestHandler<ObterQuestoesPlanoAEEPorVersaoQuery, IEnumerable<QuestaoDto>>
    {
        private readonly IMediator mediator;
        private readonly IConsultasPeriodoEscolar consultasPeriodoEscolar;

        public ObterQuestoesPlanoAEEPorVersaoQueryHandler(IMediator mediator,
                                                          IConsultasPeriodoEscolar consultasPeriodoEscolar)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.consultasPeriodoEscolar = consultasPeriodoEscolar ?? throw new ArgumentNullException(nameof(consultasPeriodoEscolar));
        }

        public async Task<IEnumerable<QuestaoDto>> Handle(ObterQuestoesPlanoAEEPorVersaoQuery request, CancellationToken cancellationToken)
        {
            var versaoPlano = request.VersaoPlanoId > 0 ?
                await mediator.Send(new ObterVersaoPlanoAEEPorIdQuery(request.VersaoPlanoId)) :
                new PlanoAEEVersaoDto();

            var ultimaVersaoPlano = await mediator.Send(new ObterUltimaVersaoPlanoAEEQuery(versaoPlano.PlanoAEEId > 0 ? versaoPlano.PlanoAEEId : 0));

            var respostasPlano = request.VersaoPlanoId > 0 ?
                await mediator.Send(new ObterRespostasPlanoAEEPorVersaoQuery(request.VersaoPlanoId)) :
                Enumerable.Empty<RespostaQuestaoDto>();

            var listaQuestoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(request.QuestionarioId, questaoId =>
               respostasPlano.Where(c => c.QuestaoId == questaoId)));

            if (!versaoPlano.Numero.Equals(ultimaVersaoPlano.Numero))
            {
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaCodigo));
                var dataUltimaAtualizacaoPlano = versaoPlano.AlteradoEm ?? versaoPlano.CriadoEm;

                dataUltimaAtualizacaoPlano = dataUltimaAtualizacaoPlano.Year < DateTime.Today.Year
                    ? new DateTime(DateTime.Today.Year, dataUltimaAtualizacaoPlano.Month, dataUltimaAtualizacaoPlano.Day)
                    : dataUltimaAtualizacaoPlano;

                var periodoEscolar = await consultasPeriodoEscolar.ObterPeriodoPorModalidade(turma.ModalidadeCodigo, dataUltimaAtualizacaoPlano);

                if (periodoEscolar == null)
                    periodoEscolar = await consultasPeriodoEscolar.ObterPeriodoAtualPorModalidade(turma.ModalidadeCodigo);

                if (listaQuestoes.SingleOrDefault(q => q.TipoQuestao == TipoQuestao.PeriodoEscolar).Resposta.Any())
                {
                    listaQuestoes.SingleOrDefault(q => q.TipoQuestao == TipoQuestao.PeriodoEscolar).Resposta.SingleOrDefault().Texto = periodoEscolar.Id.ToString();
                }
            }
            return listaQuestoes;
        }
    }
}
