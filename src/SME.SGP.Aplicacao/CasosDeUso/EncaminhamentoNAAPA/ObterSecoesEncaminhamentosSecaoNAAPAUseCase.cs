using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterSecoesEncaminhamentosSecaoNAAPAUseCase : IObterSecoesEncaminhamentosSecaoNAAPAUseCase
    {
        private readonly IMediator mediator;
        private const string SECAO_ITINERANCIA = "QUESTOES_ITINERACIA";

        public ObterSecoesEncaminhamentosSecaoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Executar(FiltroSecoesDeEncaminhamento filtro)
        {
            var secoesQuestionario = (await mediator.Send(new ObterSecoesEncaminhamentosSecaoNAAPAQuery(filtro.Modalidade, filtro.EncaminhamentoNAAPAId))).ToList();

            if (filtro.EncaminhamentoNAAPAId != 0) 
                return secoesQuestionario;
            
            foreach (var secao in secoesQuestionario)
            {
                if (secao.NomeComponente == SECAO_ITINERANCIA)
                    secao.Concluido = true;
                else
                {
                    var listaQuestoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(secao.QuestionarioId));
                    secao.Concluido = !listaQuestoes.Any(c => c.Obrigatorio);
                }
            }
            
            return secoesQuestionario;
        }
    }
}
