using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEPorCodigoEstudanteUseCase : AbstractUseCase, IObterPlanoAEEPorCodigoEstudanteUseCase
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public ObterPlanoAEEPorCodigoEstudanteUseCase(IMediator mediator, IRepositorioPlanoAEE repositorioPlanoAEE) : base(mediator)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<PlanoAEEDto> Executar(string codigoEstudante)
        {
            var plano = new PlanoAEEDto();
            var respostasPlano = Enumerable.Empty<RespostaQuestaoDto>();

            if (codigoEstudante != "")
            {
                var entidadePlano = await repositorioPlanoAEE.ObterPorIdAsync(0);

                plano.Auditoria = (AuditoriaDto)entidadePlano;
                plano.Versoes = await mediator.Send(new ObterVersoesPlanoAEEPorCodigoEstudanteQuery(codigoEstudante));

                var ultimaVersaoId = plano.Versoes
                    .OrderByDescending(a => a.Numero)
                    .Select(a => a.Id)
                    .First();

                respostasPlano = await mediator.Send(new ObterRespostasPlanoAEEPorVersaoQuery(ultimaVersaoId));
            }

            var questionarioId = await mediator.Send(new ObterQuestionarioPlanoAEEIdQuery());

            plano.Questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(questionarioId, questaoId =>
               respostasPlano.Where(c => c.QuestaoId == questaoId)));

            plano.QuestionarioId = questionarioId;

            return plano;
        }
    }
}
