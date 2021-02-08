using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEPorIdUseCase : AbstractUseCase, IObterPlanoAEEPorIdUseCase
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public ObterPlanoAEEPorIdUseCase(IMediator mediator, IRepositorioPlanoAEE repositorioPlanoAEE) : base(mediator)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<PlanoAEEDto> Executar(long? planoAEEId)
        {
            var plano = new PlanoAEEDto();
            var respostasPlano = Enumerable.Empty<RespostaQuestaoDto>();

            if (planoAEEId.HasValue)
            {
                var entidadePlano = await repositorioPlanoAEE.ObterPorIdAsync(planoAEEId.Value);

                plano.Auditoria = (AuditoriaDto)entidadePlano;
                plano.Versoes = await mediator.Send(new ObterVersoesPlanoAEEQuery(planoAEEId.Value));

                var ultimaVersaoId = plano.Versoes
                    .OrderByDescending(a => a.Numero)
                    .Select(a => a.Id)
                    .First();

                respostasPlano = await mediator.Send(new ObterRespostasPlanoAEEPorVersaoQuery(ultimaVersaoId));
            }

            var questionarioId = await mediator.Send(new ObterQuestionarioPlanoAEEIdQuery());

            plano.Questoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(questionarioId, questaoId =>
               respostasPlano.Where(c => c.QuestaoId == questaoId)));

            return plano;
        }
    }
}
