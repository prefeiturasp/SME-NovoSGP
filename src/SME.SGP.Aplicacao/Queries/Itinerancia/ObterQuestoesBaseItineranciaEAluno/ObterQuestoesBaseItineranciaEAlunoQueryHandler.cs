using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesBaseItineranciaEAlunoQueryHandler : IRequestHandler<ObterQuestoesBaseItineranciaEAlunoQuery, ItineranciaQuestoesBaseDto>
    {
        private readonly IRepositorioItinerancia repositorioItinerancia;

        public ObterQuestoesBaseItineranciaEAlunoQueryHandler(IRepositorioItinerancia repositorioItinerancia)
        {
            this.repositorioItinerancia = repositorioItinerancia ?? throw new ArgumentNullException(nameof(repositorioItinerancia));
        }

        public async Task<ItineranciaQuestoesBaseDto> Handle(ObterQuestoesBaseItineranciaEAlunoQuery request, CancellationToken cancellationToken)
        {
            var tiposQuestionario = new long[] { (int)TipoQuestionario.RegistroItinerancia, (int)TipoQuestionario.RegistroItineranciaAluno };
            var questoesBase = await repositorioItinerancia.ObterItineranciaQuestaoBase(tiposQuestionario);

            if (questoesBase == null || !questoesBase.Any())
                throw new NegocioException("Não foi possível obter as questões base da itinerância");


            var itineranciaQuestoesBase = new ItineranciaQuestoesBaseDto();

            var listaQuestao = new List<ItineranciaQuestaoDto>();
            var listaQuestaoAluno = new List<ItineranciaAlunoQuestaoDto>();


            foreach (var questaoBase in questoesBase.OrderBy(o => o.Ordem))
            {
                if (questaoBase.Tipo == TipoQuestionario.RegistroItinerancia)
                {
                    var questao = new ItineranciaQuestaoDto() { QuestaoId = questaoBase.Id, Descricao = questaoBase.Nome, Obrigatorio = questaoBase.Obrigatorio };
                    listaQuestao.Add(questao);
                }
                else
                {
                    var questaoAluno = new ItineranciaAlunoQuestaoDto() { QuestaoId = questaoBase.Id, Descricao = questaoBase.Nome, Obrigatorio = questaoBase.Obrigatorio };
                    listaQuestaoAluno.Add(questaoAluno);
                }
            }

            itineranciaQuestoesBase.ItineranciaQuestao = listaQuestao;
            itineranciaQuestoesBase.ItineranciaAlunoQuestao = listaQuestaoAluno;

            return itineranciaQuestoesBase;
        }
    }
}
