using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.IO;
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

            var dataRetornoAlterada = itinerancia.DataRetornoVerificacao != dto.DataRetornoVerificacao;
            var dataRetornoAnterior = itinerancia.DataRetornoVerificacao;

            if (itinerancia.EhNulo())
                throw new NegocioException($"Não foi possível localizar a itinerância de Id {dto.Id}");

            itinerancia.AnoLetivo = dto.AnoLetivo;
            itinerancia.DataVisita = dto.DataVisita;
            itinerancia.DataRetornoVerificacao = dto.DataRetornoVerificacao;
            itinerancia.EventoId = dto.EventoId;
            itinerancia.DreId = dto.DreId;
            itinerancia.UeId = dto.UeId;

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    var auditoriaDto = await mediator.Send(new AlterarItineranciaCommand(itinerancia));
                    if (auditoriaDto.EhNulo())
                        throw new NegocioException($"Não foi possível alterar a itinerância de Id {itinerancia.Id}");

                    await ExcluirFilhosItinerancia(dto, itinerancia);

                    await SalvarFilhosItinerancia(dto, itinerancia);

                    unitOfWork.PersistirTransacao();

                    await EnviarNotificacao(itinerancia, dto);

                    return auditoriaDto;
                }
                catch (Exception)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }

        private async Task EnviarNotificacao(Itinerancia itinerancia, ItineranciaDto dto)
        {
            var verificaWorkflow = await mediator.Send(new ObterWorkflowItineranciaPorItineranciaIdQuery(itinerancia.Id));
            WorkflowAprovacao workflow = null;

            if (verificaWorkflow.NaoEhNulo())
                workflow = await mediator.Send(new ObterWorkflowPorIdQuery(verificaWorkflow.WfAprovacaoId));

            if (workflow.EhNulo() || workflow.Niveis.Any(a => a.Status == WorkflowAprovacaoNivelStatus.Reprovado))
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAEE.RotaNotificacaoRegistroItineranciaInseridoUseCase,
                    new NotificacaoSalvarItineranciaDto
                    {
                        CriadoRF = itinerancia.CriadoRF,
                        CriadoPor = itinerancia.CriadoPor,
                        DataVisita = dto.DataVisita,
                        UeId = dto.UeId,
                        Estudantes = dto.Alunos,
                        ItineranciaId = itinerancia.Id
                    }, Guid.NewGuid(), null));
        }

        public async Task<bool> ExcluirFilhosItinerancia(ItineranciaDto itineranciaDto, Itinerancia itinerancia)
        {
            await ExcluirItineranciaAlunos(itineranciaDto, itinerancia);
            await ExcluirItineranciaObjetivos(itineranciaDto, itinerancia);
            await ExcluirItineranciaQuestoes(itineranciaDto, itinerancia);
            return true;
        }

        private async Task ExcluirItineranciaQuestaoUpload(ItineranciaQuestaoDto questao)
        {
            if (questao.Excluido && questao.QuestaoTipoUploadRespondida())
            {
                var arquivoCodigo = Guid.Parse(questao.Resposta);
                await ExcluirArquivoItinerancia(arquivoCodigo);
            }
        }

        private async Task ExcluirItineranciaQuestoes(ItineranciaDto itineranciaDto, Itinerancia itinerancia)
        {
            if (itineranciaDto.PossuiQuestoes)
                foreach (var questao in itineranciaDto.Questoes)
                {
                    if (questao.Id != 0 &&
                        !await mediator.Send(new ExcluirItineranciaQuestaoCommand(questao.Id, itinerancia.Id)))
                            throw new NegocioException($"Não foi possível excluir a questão da itinerância de Id {questao.Id}");

                    await ExcluirItineranciaQuestaoUpload(questao);
                }
        }

        private async Task ExcluirItineranciaObjetivos(ItineranciaDto itineranciaDto, Itinerancia itinerancia)
        {
            if (itineranciaDto.PossuiObjetivos)
                foreach (var objetivo in itinerancia.ObjetivosVisita)
                    if (!await mediator.Send(new ExcluirItineranciaObjetivoCommand(objetivo.Id, itinerancia.Id)))
                        throw new NegocioException($"Não foi possível excluir o objetivo da itinerância de Id {objetivo.Id}");
        }

        private async Task ExcluirItineranciaAlunos(ItineranciaDto itineranciaDto, Itinerancia itinerancia)
        {
            if (itineranciaDto.PossuiAlunos)
                foreach (var aluno in itinerancia.Alunos)
                    if (!await mediator.Send(new ExcluirItineranciaAlunoCommand(aluno)))
                        throw new NegocioException($"Não foi possível excluir a itinerância do aluno de Id {aluno.Id}");
        }

        public async Task<bool> SalvarFilhosItinerancia(ItineranciaDto itineranciaDto, Itinerancia itinerancia)
        {
            await SalvarItineranciaAlunos(itineranciaDto, itinerancia);
            await SalvarItineranciaObjetivos(itineranciaDto, itinerancia);
            await SalvarItineranciaQuestoes(itineranciaDto, itinerancia);
            return true;
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

        private async Task SalvarItineranciaObjetivos(ItineranciaDto itineranciaDto, Itinerancia itinerancia)
        {
            if (itineranciaDto.PossuiObjetivos)
                foreach (var objetivo in itineranciaDto.ObjetivosVisita)
                    await mediator.Send(new SalvarItineranciaObjetivoCommand(objetivo.ItineranciaObjetivoBaseId, itinerancia.Id, objetivo.Descricao, objetivo.TemDescricao));
        }

        private async Task SalvarItineranciaQuestoes(ItineranciaDto itineranciaDto, Itinerancia itinerancia)
        {
            if (itineranciaDto.PossuiQuestoes)
                foreach (var questao in itineranciaDto.Questoes)
                {
                    if (!questao.Excluido)
                    {
                        await SalvarItineranciaQuestaoUpload(questao);
                        if (questao.QuestaoTipoTexto() || questao.QuestaoTipoUploadRespondida())
                            await mediator.Send(new SalvarItineranciaQuestaoCommand(questao.QuestaoId, itinerancia.Id, questao.Resposta, questao.ArquivoId));
                    }
                }
        }

        private async Task SalvarItineranciaAlunos(ItineranciaDto itineranciaDto, Itinerancia itinerancia)
        {
            if (itineranciaDto.PossuiAlunos)
            {
                await TrataTurmasCodigos(itineranciaDto);
                foreach (var aluno in itineranciaDto.Alunos)
                    await mediator.Send(new SalvarItineranciaAlunoCommand(aluno, itinerancia.Id));
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

        private async Task ExcluirArquivoItinerancia(Guid arquivoCodigo)
        {
            var entidadeArquivo = await mediator.Send(new ObterArquivoPorCodigoQuery(arquivoCodigo));
            if (entidadeArquivo.EhNulo())
                throw new NegocioException(MensagemNegocioComuns.ARQUIVO_INF0RMADO_NAO_ENCONTRADO);

            await mediator.Send(new ExcluirArquivoRepositorioPorIdCommand(entidadeArquivo.Id));

            var extencao = Path.GetExtension(entidadeArquivo.Nome);
            var filtro = new FiltroExcluirArquivoArmazenamentoDto { ArquivoNome = entidadeArquivo.Codigo.ToString() + extencao };
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverArquivoArmazenamento, filtro, Guid.NewGuid(), null));
        }
    }
}
