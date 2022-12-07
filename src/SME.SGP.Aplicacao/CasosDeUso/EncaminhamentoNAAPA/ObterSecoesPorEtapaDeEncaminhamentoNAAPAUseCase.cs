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
    public class ObterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase : IObterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterSecoesPorEtapaDeEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Executar(FiltroSecoesPorEtapaDeEncaminhamento filtro)
        {
            var listaEtapas = new List<int> { (int)EtapaEncaminhamentoNAAPA.PrimeiraEtapa };

            var secoesQuestionario = (await mediator.Send(new ObterSecoesPorEtapaDeEncaminhamentoNAAPAQuery(listaEtapas,
                filtro.EncaminhamentoNAAPAId, filtro.Modalidade))).ToList();

            if (filtro.EncaminhamentoNAAPAId != 0) 
                return secoesQuestionario;
            
            foreach (var secao in secoesQuestionario)
            {
                var listaQuestoes = await mediator.Send(new ObterQuestoesPorQuestionarioPorIdQuery(secao.QuestionarioId));
                secao.Concluido = !listaQuestoes.Any(c => c.Obrigatorio);
            }
            
            return secoesQuestionario;
        }
    }
}
