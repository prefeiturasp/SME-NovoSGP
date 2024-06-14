using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class
        PersistirRelatorioPAPCommandHandler : IRequestHandler<PersistirRelatorioPAPCommand, ResultadoRelatorioPAPDto>
    {
        private readonly IMediator mediator;

        public PersistirRelatorioPAPCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ResultadoRelatorioPAPDto> Handle(PersistirRelatorioPAPCommand request,
            CancellationToken cancellationToken)
        {
            var relatorioPAPDto = request.RelatorioPAPDto;
            var turmaCodigo = await mediator.Send(new ObterTurmaCodigoPorIdQuery(relatorioPAPDto.TurmaId));
            if (string.IsNullOrEmpty(turmaCodigo))
                throw new NegocioException(MensagemNegocioTurma.TURMA_NAO_ENCONTRADA);

            var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(relatorioPAPDto.AlunoCodigo,
                DateTime.Now.Year));
            if (aluno.EhNulo())
                throw new NegocioException(MensagemNegocioAluno.ESTUDANTE_NAO_ENCONTRADO);

            await ValidarQuestoesObrigatorias(relatorioPAPDto, turmaCodigo);

            if (relatorioPAPDto.PAPTurmaId.HasValue)
                return await mediator.Send(new AlterarRelatorioPAPCommand(relatorioPAPDto));

            return await mediator.Send(new SalvarRelatorioPAPCommand(relatorioPAPDto));
        }

        private async Task ValidarQuestoesObrigatorias(RelatorioPAPDto relatorioPAPDto, string turmaCodigo)
        {
            var questoesObrigatoriasAConsistir =
                await ObterQuestoesObrigatoriasNaoPreechidas(relatorioPAPDto, turmaCodigo);
            if (questoesObrigatoriasAConsistir.Any())
            {
                var mensagem = questoesObrigatoriasAConsistir.GroupBy(questao => questao.SecaoNome).Select(secao =>
                        $"Seção: {secao.Key} Questões: [{string.Join(", ", secao.Select(questao => questao.QuestaoOrdem).Distinct().ToArray())}]")
                    .ToList();

                throw new NegocioException(string.Format(
                    MensagemNegocioRelatorioPAP.EXISTEM_QUESTOES_OBRIGATORIAS_NAO_PREENCHIDAS,
                    string.Join(", ", mensagem)));
            }
        }

        private async Task<List<QuestaoObrigatoriaNaoRespondidaDto>> ObterQuestoesObrigatoriasNaoPreechidas(
            RelatorioPAPDto relatorioPAPDto, string turmaCodigo)
        {
            List<QuestaoObrigatoriaNaoRespondidaDto> questoesObrigatoriasAConsistir =
                new List<QuestaoObrigatoriaNaoRespondidaDto>();
            var secoesEtapa = await mediator.Send(new ObterSecoesPAPQuery(turmaCodigo, relatorioPAPDto.AlunoCodigo,
                relatorioPAPDto.periodoRelatorioPAPId));
            IEnumerable<RespostaQuestaoObrigatoriaDto> respostasPersistidas = null;

            foreach (var secao in secoesEtapa.Secoes)
            {
                var secaoPresenteDto = relatorioPAPDto.Secoes.FirstOrDefault(secaoDto => secaoDto.SecaoId == secao.Id);

                IEnumerable<RespostaQuestaoObrigatoriaDto> respostasSecao;
                if (secaoPresenteDto.NaoEhNulo() && secaoPresenteDto.Respostas.Any())
                {
                    respostasSecao = secaoPresenteDto.Respostas
                        .Select(questao => new RespostaQuestaoObrigatoriaDto()
                        {
                            QuestaoId = questao.QuestaoId,
                            Resposta = questao.Resposta
                        })
                        .Where(questao => !string.IsNullOrEmpty(questao.Resposta));
                }
                else
                {
                    if (respostasPersistidas.EhNulo())
                        respostasPersistidas = await ObterRespostasPersistidas(secao.PAPSecaoId);
                    respostasSecao = respostasPersistidas;
                }

                var secaoQuestionario = new SecaoQuestionarioDto()
                {
                    Id = secao.Id,
                    Nome = secao.Nome,
                    QuestionarioId = secao.QuestionarioId,
                    Auditoria = secao.Auditoria
                };

                questoesObrigatoriasAConsistir.AddRange(await mediator.Send(
                    new ObterQuestoesObrigatoriasNaoRespondidasQuery(secaoQuestionario, respostasSecao)));
            }

            return questoesObrigatoriasAConsistir;
        }
        private async Task<IEnumerable<RespostaQuestaoObrigatoriaDto>> ObterRespostasPersistidas(long? PAPSecaoId)
        {
            if (PAPSecaoId.HasValue)
                return (await mediator.Send(new ObterRespostaPorSecaoPAPQuery(PAPSecaoId.Value)))
                    .Select(resposta => new RespostaQuestaoObrigatoriaDto
                    {
                        QuestaoId = resposta.RelatorioPeriodicoQuestao.QuestaoId,
                        Resposta = resposta.RespostaId.HasValue ? resposta.RespostaId?.ToString() : resposta.Texto,
                        Persistida = true
                    });

            return Enumerable.Empty<RespostaQuestaoObrigatoriaDto>();
        }
    }
}