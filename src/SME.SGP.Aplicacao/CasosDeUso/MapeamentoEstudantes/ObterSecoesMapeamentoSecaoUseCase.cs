using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterSecoesMapeamentoSecaoUseCase : IObterSecoesMapeamentoSecaoUseCase
    {
        private readonly IMediator mediator;
        
        public ObterSecoesMapeamentoSecaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Executar(long? mapeamentoEstudanteId)
        {
            var secoesQuestionario = (await mediator.Send(new ObterSecoesMapeamentoEstudanteSecaoQuery(mapeamentoEstudanteId))).ToList();
            foreach (var secao in secoesQuestionario.Where(secao => secao.Auditoria.EhNulo()))
            {
                var listaQuestoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(secao.QuestionarioId));
                secao.Concluido = !listaQuestoes.Any(c => c.Obrigatorio);
            }
            
            return secoesQuestionario;
        }
    }
}
