using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirItineranciaUseCase : AbstractUseCase, IExcluirItineranciaUseCase
    {
        private readonly IUnitOfWork unitOfWork;

        public ExcluirItineranciaUseCase(IUnitOfWork unitOfWork, IMediator mediator) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<bool> Executar(long id)
        {
            return ExluirItinerancia(id);
        }

        public async Task<bool> ExluirItinerancia(long id)
        {
            var itinerancia = await mediator.Send(new ObterItineranciaPorIdQuery(id));

            if (itinerancia == null)
                throw new NegocioException($"Não foi possível localizar a itinerância de Id {id}");

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    if(!await mediator.Send(new ExcluirItineranciaCommand(itinerancia.Id)))
                        throw new NegocioException($"Não foi possível excluir a itinerância de Id {itinerancia.Id}");

                    foreach (var aluno in itinerancia.Alunos)                    
                        if(!await mediator.Send(new ExcluirItineranciaAlunoCommand(aluno)))
                            throw new NegocioException($"Não foi possível excluir a itinerância do aluno de Id {aluno.Id}");

                    foreach (var objetivo in itinerancia.ObjetivosVisita)
                        if(!await mediator.Send(new ExcluirItineranciaObjetivoCommand(objetivo.Id, itinerancia.Id)))
                            throw new NegocioException($"Não foi possível excluir o objetivo da itinerância de Id {objetivo.Id}");

                    foreach (var questao in itinerancia.Questoes)
                        if(!await mediator.Send(new ExcluirItineranciaQuestaoCommand(questao.Id, itinerancia.Id)))
                            throw new NegocioException($"Não foi possível excluir a quesão da itinerância de Id {questao.Id}");

                    unitOfWork.PersistirTransacao();

                    return true;
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
