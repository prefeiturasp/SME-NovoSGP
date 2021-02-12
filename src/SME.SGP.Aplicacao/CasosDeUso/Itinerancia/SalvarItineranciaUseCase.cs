using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarItineranciaUseCase : AbstractUseCase, ISalvarItineranciaUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public SalvarItineranciaUseCase(IUnitOfWork unitOfWork, IMediator mediator) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<AuditoriaDto> Executar(ItineranciaDto itineranciaDto)
        {            
            return await SalvarItinerancia(itineranciaDto);
        }

        public async Task<AuditoriaDto> SalvarItinerancia(ItineranciaDto itineranciaDto)
        {
            var itinerancia = await mediator.Send(new SalvarItineranciaCommand(itineranciaDto.DataVisita, itineranciaDto.DataRetornoVerificacao));
            if (itinerancia == null)
                throw new NegocioException("Erro ao Salvar a itinerancia");

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {   
                    foreach (var aluno in itineranciaDto.Alunos)
                        await mediator.Send(new SalvarItineranciaAlunoCommand(aluno, itinerancia.Id));
                    
                    foreach (var objetivo in itineranciaDto.ObjetivosVisita)
                        await mediator.Send(new SalvarItineranciaObjetivoCommand(objetivo.ItineranciaObjetivoId, itinerancia.Id, objetivo.Descricao));
                                        
                    foreach (var questao in itineranciaDto.Questoes)
                        await mediator.Send(new SalvarItineranciaQuestaoCommand(questao.QuestaoId, itinerancia.Id, questao.Resposta));
                                       
                    foreach (var ue in itineranciaDto.Ues)
                        await mediator.Send(new SalvarItineranciaUeCommand(ue.UeId, itinerancia.Id));

                    unitOfWork.PersistirTransacao();

                    return itinerancia;
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }
    }
}
