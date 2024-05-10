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
    public class AlterarRelatorioPAPCommandHandler : IRequestHandler<AlterarRelatorioPAPCommand,ResultadoRelatorioPAPDto>
    {
        private readonly IMediator mediator;

        public AlterarRelatorioPAPCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ResultadoRelatorioPAPDto> Handle(AlterarRelatorioPAPCommand request, CancellationToken cancellationToken)
        {
            var relatorioPAPDto = request.RelatorioPAPDto;
            var resultado = new ResultadoRelatorioPAPDto();
            var relatorioAluno = await mediator.Send(new PersistirRelatorioAlunoCommand(relatorioPAPDto, relatorioPAPDto.PAPTurmaId.Value));

            resultado.PAPTurmaId = relatorioPAPDto.PAPTurmaId.Value;
            resultado.PAPAlunoId = relatorioAluno.Id;

            foreach (var secao in relatorioPAPDto.Secoes)
            {
                resultado.Secoes.Add(await AlterarSecao(secao, relatorioAluno.Id));
            }

            return resultado;
        }
        private async Task<ResultadoRelatorioPAPSecaoDto> AlterarSecao(RelatorioPAPSecaoDto secao, long relatorioAlunoId)
        {
            var relatorioSecao = await mediator.Send(new PersistirRelatorioSecaoCommand(secao, relatorioAlunoId));
            
            foreach (var questaoRespostas in secao.Respostas.GroupBy(q => q.QuestaoId))
            {
                var questaoExistente = relatorioSecao.Questoes.FirstOrDefault(q => q.QuestaoId == questaoRespostas.Key);

                if (questaoExistente.EhNulo())
                    await mediator.Send(new SalvarQuestaoCommand(relatorioSecao.Id, questaoRespostas.Key, questaoRespostas));
                else
                    await AlterarQuestao(questaoExistente, questaoRespostas);
            }

            return new ResultadoRelatorioPAPSecaoDto()
            {
                SecaoId = relatorioSecao.Id,
                Auditoria = new AuditoriaDto()
                {
                    Id = secao.SecaoId,
                    CriadoEm = relatorioSecao.CriadoEm,
                    CriadoPor = relatorioSecao.CriadoPor,
                    CriadoRF = relatorioSecao.CriadoRF,
                    AlteradoEm = relatorioSecao.AlteradoEm,
                    AlteradoPor = relatorioSecao.AlteradoPor,
                    AlteradoRF = relatorioSecao.AlteradoRF
                }
            }; 
        }
        
          private async Task AlterarQuestao(RelatorioPeriodicoPAPQuestao questaoExistente, IEnumerable<RelatorioPAPRespostaDto> respostas)
        {
            if (questaoExistente.Excluido)
                await AlterarQuestaoExcluida(questaoExistente);

            await ExcluirRespostasEncaminhamento(questaoExistente, respostas);

            await AlterarRespostasEncaminhamento(questaoExistente, respostas);

            await IncluirRespostasEncaminhamento(questaoExistente, respostas);
        }

        private async Task AlterarQuestaoExcluida(RelatorioPeriodicoPAPQuestao questao)
        {
            questao.Excluido = false;
            await mediator.Send(new AlterarRelatorioPeriodicoQuestaoPAPCommand(questao));
        }

        private async Task AlterarRespostasEncaminhamento(RelatorioPeriodicoPAPQuestao questaoExistente, IEnumerable<RelatorioPAPRespostaDto> respostas)
        {
            var repostasAlteradas = questaoExistente.Respostas.Where(s => respostas.Any(c => c.RelatorioRespostaId == s.Id));
            
            foreach (var respostaAlterar in repostasAlteradas)
                await mediator.Send(new AlterarRelatorioPeriodicoRespostaPAPCommand(respostaAlterar, respostas.FirstOrDefault(c => c.RelatorioRespostaId == respostaAlterar.Id)));
        }

        private async Task ExcluirRespostasEncaminhamento(RelatorioPeriodicoPAPQuestao questoesExistentes, IEnumerable<RelatorioPAPRespostaDto> respostas)
        {
            var respostasExcluidas = questoesExistentes.Respostas.Where(s => !respostas.Any(c => c.RelatorioRespostaId == s.Id));

            foreach (var respostasExcluir in respostasExcluidas)
                await mediator.Send(new ExcluirRelatorioPeriodicoRespostaPAPCommand(respostasExcluir));
        }

        private async Task IncluirRespostasEncaminhamento(RelatorioPeriodicoPAPQuestao questaoExistente, IEnumerable<RelatorioPAPRespostaDto> respostas)
        {
            var repostasIncluidas = respostas.Where(c => !c.RelatorioRespostaId.HasValue);

            foreach (var questao in repostasIncluidas)
                await mediator.Send(new SalvarRelatorioPeriodicoRespostaPAPCommand(questao.Resposta, questao.TipoQuestao, questaoExistente.Id));

        }





    }
}