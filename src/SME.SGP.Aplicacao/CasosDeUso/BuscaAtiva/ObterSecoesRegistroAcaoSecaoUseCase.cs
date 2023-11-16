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
    public class ObterSecoesRegistroAcaoSecaoUseCase : IObterSecoesRegistroAcaoSecaoUseCase
    {
        private readonly IMediator mediator;
        
        public ObterSecoesRegistroAcaoSecaoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Executar(FiltroSecoesDeRegistroAcao filtro)
        {
            var secoesQuestionario = (await mediator.Send(new ObterSecoesRegistroAcaoSecaoQuery(filtro.RegistroAcaoBuscaAtivaId))).ToList();
            foreach (var secao in secoesQuestionario.Where(secao => secao.Auditoria.EhNulo()))
            {
                var listaQuestoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(secao.QuestionarioId));
                secao.Concluido = !listaQuestoes.Any(c => c.Obrigatorio);
            }
            
            return secoesQuestionario;
        }
    }
}
