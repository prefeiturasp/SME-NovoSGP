using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public class AlterarItineranciaUseCase : AbstractUseCase, IAlterarItineranciaUseCase
    {
        private readonly IUnitOfWork unitOfWork;
        public AlterarItineranciaUseCase(IUnitOfWork unitOfWork, IMediator mediator) : base(mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<AuditoriaDto> Executar(ItineranciaDto dto)
        {
            var itinerancia = await mediator.Send(new ObterItineranciaPorIdQuery(dto.Id));

            if (itinerancia == null)
                throw new NegocioException($"Não foi possível localizar a itinerância de Id {dto.Id}");

            itinerancia.DataVisita = dto.DataVisita;
            itinerancia.DataRetornoVerificacao = dto.DataRetornoVerificacao;

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var auditoriaDto = await mediator.Send(new AlterarItineranciaCommand(itinerancia));
                    if(auditoriaDto == null)
                        throw new NegocioException($"Não foi possível alterar a itinerância de Id {itinerancia.Id}");
                    
                    await ExluirFilhosItinerancia(itinerancia);
                    
                    await SalvarFilhosItinerancia(dto, itinerancia);

                    unitOfWork.PersistirTransacao();

                    return auditoriaDto;
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

        }

        public async Task<bool> ExluirFilhosItinerancia(Itinerancia itinerancia)
        {
            if (itinerancia.Alunos == null || itinerancia.Alunos.Any())
                foreach (var aluno in itinerancia.Alunos)
                    if (!await mediator.Send(new ExcluirItineranciaAlunoCommand(aluno)))
                        throw new NegocioException($"Não foi possível excluir a itinerância do aluno de Id {aluno.Id}");

            if (itinerancia.ObjetivosVisita == null || itinerancia.ObjetivosVisita.Any())
                foreach (var objetivo in itinerancia.ObjetivosVisita)
                    if (!await mediator.Send(new ExcluirItineranciaObjetivoCommand(objetivo.Id)))
                        throw new NegocioException($"Não foi possível excluir o objetivo da itinerância de Id {objetivo.Id}");

            if (itinerancia.Questoes == null || itinerancia.Questoes.Any())
                foreach (var questao in itinerancia.Questoes)
                    if (!await mediator.Send(new ExcluirItineranciaQuestaoCommand(questao.Id)))
                        throw new NegocioException($"Não foi possível excluir a quesão da itinerância de Id {questao.Id}");

            if (itinerancia.Ues == null || itinerancia.Ues.Any())
                foreach (var ue in itinerancia.Ues)
                    if (!await mediator.Send(new ExcluirItineranciaUeCommand(ue.Id)))
                        throw new NegocioException($"Não foi possível excluir a ue da itinerância de Id {ue.Id}");

            return true;
        }

        public async Task<bool> SalvarFilhosItinerancia(ItineranciaDto itineranciaDto, Itinerancia itinerancia)
        {
            if (itineranciaDto.Alunos == null || itineranciaDto.Alunos.Any())
                foreach (var aluno in itineranciaDto.Alunos)
                    await mediator.Send(new SalvarItineranciaAlunoCommand(aluno, itinerancia.Id));

            if (itineranciaDto.ObjetivosVisita == null || itineranciaDto.ObjetivosVisita.Any())
                foreach (var objetivo in itineranciaDto.ObjetivosVisita)
                    await mediator.Send(new SalvarItineranciaObjetivoCommand(objetivo.ItineranciaObjetivoBaseId, itinerancia.Id, objetivo.Descricao, objetivo.TemDescricao));

            if (itineranciaDto.Questoes == null || itineranciaDto.Questoes.Any())
                foreach (var questao in itineranciaDto.Questoes)
                    await mediator.Send(new SalvarItineranciaQuestaoCommand(questao.QuestaoId, itinerancia.Id, questao.Resposta));

            if (itineranciaDto.Ues == null || itineranciaDto.Ues.Any())
                foreach (var ue in itineranciaDto.Ues)
                    await mediator.Send(new SalvarItineranciaUeCommand(ue.UeId, itinerancia.Id));

            return true;
        }
    }
}
