using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
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
                    if (itineranciaDto.Alunos.Any())
                    {
                        await TrataTurmasCodigos(itineranciaDto);
                    }
                    var itinerancia = await mediator.Send(new SalvarItineranciaCommand(itineranciaDto.AnoLetivo, itineranciaDto.DataVisita, itineranciaDto.DataRetornoVerificacao, itineranciaDto.EventoId));
                    if (itinerancia == null)
                        throw new NegocioException("Erro ao Salvar a itinerancia");

                    if (itineranciaDto.PossuiAlunos)
                        foreach (var aluno in itineranciaDto.Alunos)
                            await mediator.Send(new SalvarItineranciaAlunoCommand(aluno, itinerancia.Id));

                    if (itineranciaDto.PossuiObjetivos)
                        foreach (var objetivo in itineranciaDto.ObjetivosVisita)
                            await mediator.Send(new SalvarItineranciaObjetivoCommand(objetivo.ItineranciaObjetivoBaseId, itinerancia.Id, objetivo.Descricao, objetivo.TemDescricao));

                    if (itineranciaDto.PossuiQuestoes)
                        foreach (var questao in itineranciaDto.Questoes)
                            await mediator.Send(new SalvarItineranciaQuestaoCommand(questao.QuestaoId, itinerancia.Id, questao.Resposta));

                    if (itineranciaDto.PossuiUes)
                        foreach (var ue in itineranciaDto.Ues)
                            await mediator.Send(new SalvarItineranciaUeCommand(ue.UeId, itinerancia.Id));

                    if (itineranciaDto.DataRetornoVerificacao.HasValue && !itineranciaDto.PossuiAlunos)
                        await SalvarEventoItinerancia(itinerancia.Id, itineranciaDto);

                    unitOfWork.PersistirTransacao();

                    SentrySdk.AddBreadcrumb($"Mensagem RotaNotificacaoRegistroItineranciaInseridoUseCase", "Rabbit - RotaNotificacaoRegistroItineranciaInseridoUseCase");
                    await mediator.Send(new AlterarSituacaoItineranciaCommand(itinerancia.Id, Dominio.Enumerados.SituacaoItinerancia.Enviado));
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.RotaNotificacaoRegistroItineranciaInseridoUseCase,
                        new NotificacaoSalvarItineranciaDto
                        {
                            CriadoRF = itinerancia.CriadoRF,
                            CriadoPor = itinerancia.CriadoPor,
                            DataVisita = itineranciaDto.DataVisita,
                            Ues = itineranciaDto.Ues,
                            Estudantes = itineranciaDto.Alunos,
                            ItineranciaId = itinerancia.Id
                        }, Guid.NewGuid(), null));

                    return itinerancia;
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task SalvarEventoItinerancia(long itineranciaId, ItineranciaDto itineranciaDto)
        {
            foreach (var ue in itineranciaDto.Ues)
                await mediator.Send(new CriarEventoItineranciaPAAICommand(
                    itineranciaId,
                    ue.CodigoDre,
                    ue.CodigoUe,
                    itineranciaDto.DataRetornoVerificacao.Value,
                    itineranciaDto.DataVisita,
                    ObterObjetivos(itineranciaDto.ObjetivosVisita)));
        }

        private IEnumerable<ItineranciaObjetivoDescricaoDto> ObterObjetivos(IEnumerable<ItineranciaObjetivoDto> objetivosVisita)
            => objetivosVisita.Select(a => (ItineranciaObjetivoDescricaoDto)a);

        private async Task TrataTurmasCodigos(ItineranciaDto itineranciaDto)
        {
            var turmasCodigos = itineranciaDto.Alunos.Select(a => a.TurmaId.ToString()).Distinct().ToList();

            if (turmasCodigos != null && turmasCodigos.Any())
            {
                var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigos.ToArray()));
                if (turmas.Count() != turmasCodigos.Count())
                    throw new NegocioException("Não foi possível localizar as turmas no SGP.");

                foreach (var item in itineranciaDto.Alunos)
                {
                    item.TurmaId = turmas.FirstOrDefault(a => a.CodigoTurma == item.TurmaId.ToString()).Id;
                }
            }
        }
    }
}
