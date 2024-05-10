﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
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

        private async Task SalvarItineranciaAlunos(long itineranciaId, ItineranciaDto itineranciaDto)
        {
            if (itineranciaDto.PossuiAlunos)
                foreach (var aluno in itineranciaDto.Alunos)
                    await mediator.Send(new SalvarItineranciaAlunoCommand(aluno, itineranciaId));
        }

        private async Task SalvarItineranciaObjetivos(long itineranciaId, ItineranciaDto itineranciaDto)
        {
            if (itineranciaDto.PossuiObjetivos)
                foreach (var objetivo in itineranciaDto.ObjetivosVisita)
                    await mediator.Send(new SalvarItineranciaObjetivoCommand(objetivo.ItineranciaObjetivoBaseId, itineranciaId, objetivo.Descricao, objetivo.TemDescricao));
        }

        private async Task SalvarItineranciaQuestoes(long itineranciaId, ItineranciaDto itineranciaDto)
        {
            if (itineranciaDto.PossuiQuestoes)
                foreach (var questao in itineranciaDto.Questoes)
                {
                    await SalvarItineranciaQuestaoUpload(questao);
                    if (questao.QuestaoTipoTexto() || questao.QuestaoTipoUploadRespondida())
                        await mediator.Send(new SalvarItineranciaQuestaoCommand(questao.QuestaoId, itineranciaId, questao.Resposta, questao.ArquivoId));
                }
        }

        private async Task SalvarItineranciaQuestaoUpload(ItineranciaQuestaoDto questao)
        {
            if (questao.QuestaoTipoUploadRespondida() &&
                        questao.QuestaoSemArquivoId())
            {
                var arquivoCodigo = Guid.Parse(questao.Resposta);
                questao.ArquivoId = await mediator.Send(new ObterArquivoIdPorCodigoQuery(arquivoCodigo));
            }
        }

        public async Task<AuditoriaDto> SalvarItinerancia(ItineranciaDto itineranciaDto)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    if (itineranciaDto.Alunos.Any())
                        await TrataTurmasCodigos(itineranciaDto);
                    var itinerancia = await mediator.Send(new SalvarItineranciaCommand(itineranciaDto.AnoLetivo, itineranciaDto.DataVisita, itineranciaDto.DataRetornoVerificacao, itineranciaDto.EventoId, itineranciaDto.DreId, itineranciaDto.UeId));
                    if (itinerancia.EhNulo())
                        throw new NegocioException("Erro ao Salvar a itinerancia");

                    await SalvarItineranciaAlunos(itinerancia.Id, itineranciaDto);
                    await SalvarItineranciaObjetivos(itinerancia.Id, itineranciaDto);
                    await SalvarItineranciaQuestoes(itinerancia.Id, itineranciaDto);
                    unitOfWork.PersistirTransacao();

                    await mediator.Send(new AlterarSituacaoItineranciaCommand(itinerancia.Id, Dominio.Enumerados.SituacaoItinerancia.Enviado));
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAEE.RotaNotificacaoRegistroItineranciaInseridoUseCase,
                        new NotificacaoSalvarItineranciaDto
                        {
                            CriadoRF = itinerancia.CriadoRF,
                            CriadoPor = itinerancia.CriadoPor,
                            DataVisita = itineranciaDto.DataVisita,
                            Estudantes = itineranciaDto.Alunos,
                            ItineranciaId = itinerancia.Id,
                            UeId = itineranciaDto.UeId,
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

        private async Task TrataTurmasCodigos(ItineranciaDto itineranciaDto)
        {
            var turmasCodigos = itineranciaDto.Alunos.Select(a => a.TurmaId.ToString()).Distinct().ToList();

            if (turmasCodigos.NaoEhNulo() && turmasCodigos.Any())
            {
                var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(turmasCodigos.ToArray()));
                if (turmas.Count() != turmasCodigos.Count)
                    throw new NegocioException("Não foi possível localizar as turmas no SGP.");

                foreach (var item in itineranciaDto.Alunos)
                {
                    item.TurmaId = turmas.FirstOrDefault(a => a.CodigoTurma == item.TurmaId.ToString()).Id;
                }
            }
        }
    }
}
