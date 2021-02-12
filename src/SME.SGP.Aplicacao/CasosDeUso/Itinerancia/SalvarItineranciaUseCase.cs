using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
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
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var itinerancia = await mediator.Send(new SalvarItineranciaCommand(itineranciaDto.DataVisita, itineranciaDto.DataRetornoVerificacao));
                    if (itinerancia == null)
                        throw new NegocioException("Erro ao Salvar a itinerancia");

                    if (itineranciaDto.Alunos == null || itineranciaDto.Alunos.Any())
                        foreach (var aluno in itineranciaDto.Alunos)
                            await mediator.Send(new SalvarItineranciaAlunoCommand(aluno, itinerancia.Id));

                    if (itineranciaDto.ObjetivosVisita == null || itineranciaDto.ObjetivosVisita.Any())
                        foreach (var objetivo in itineranciaDto.ObjetivosVisita)
                            await mediator.Send(new SalvarItineranciaObjetivoCommand(objetivo.ItineranciaObjetivoBaseId, itinerancia.Id, objetivo.Descricao));

                    if (itineranciaDto.Questoes == null || itineranciaDto.Questoes.Any())
                        foreach (var questao in itineranciaDto.Questoes)
                            await mediator.Send(new SalvarItineranciaQuestaoCommand(questao.QuestaoId, itinerancia.Id, questao.Resposta));

                    if (itineranciaDto.Ues == null || itineranciaDto.Ues.Any())
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
