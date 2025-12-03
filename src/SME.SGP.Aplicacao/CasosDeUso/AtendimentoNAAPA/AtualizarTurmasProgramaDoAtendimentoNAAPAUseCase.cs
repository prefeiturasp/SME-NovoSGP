using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarTurmasProgramaDoAtendimentoNAAPAUseCase : AbstractUseCase, IAtualizarTurmasProgramaDoAtendimentoNAAPAUseCase
    {
        public AtualizarTurmasProgramaDoAtendimentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var encaminhamentoNAAPADto = param.ObterObjetoMensagem<AtendimentoNAAPADto>();
            var turma = (await mediator.Send(new ObterTurmaPorIdQuery(encaminhamentoNAAPADto.TurmaId)));

            var turmasProgramaAluno = (await mediator.Send(new ObterTurmasProgramaAlunoQuery(encaminhamentoNAAPADto.AlunoCodigo, turma.AnoLetivo, true)))
                                        .Select(turmaPrograma => MapearDTO(turmaPrograma)).ToList();

            var questaoTurmasProgramaNAAPA = (await mediator.Send(new ObterQuestaoTurmasProgramaEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPADto.Id ?? 0)));
            if (questaoTurmasProgramaNAAPA.EhNulo()) return false;

            var respostaTurmasProgramaNAAPA = questaoTurmasProgramaNAAPA.Respostas?.FirstOrDefault();
            if (respostaTurmasProgramaNAAPA.NaoEhNulo())
            {
                var turmasProgramaNaapa = JsonConvert.DeserializeObject<List<RespostaTurmaProgramaAtendimentoNAAPADto>>(respostaTurmasProgramaNAAPA?.Texto);
                if (turmasProgramaAluno.Count == turmasProgramaNaapa?.Count &&
                    turmasProgramaAluno.All(turmaProgramaAluno => turmasProgramaNaapa.NaoEhNulo() && turmasProgramaNaapa.Any(x => x.EhIgual(turmaProgramaAluno)))) 
                    return false;

                var respostaEnderecoAtualizado = MapearDTO(questaoTurmasProgramaNAAPA.QuestaoId, respostaTurmasProgramaNAAPA?.Id ?? 0, turmasProgramaAluno);
                return await mediator.Send(new AlterarAtendimentoNAAPASecaoQuestaoRespostaCommand(respostaTurmasProgramaNAAPA,
                                                                                                  respostaEnderecoAtualizado));
            }
            else
                return (await mediator.Send(new RegistrarAtendimentoNAAPASecaoQuestaoRespostaCommand(JsonConvert.SerializeObject(turmasProgramaAluno),
                                                                                                      questaoTurmasProgramaNAAPA.QuestaoId, TipoQuestao.TurmasPrograma))) != 0;  
        }

        private RespostaTurmaProgramaAtendimentoNAAPADto MapearDTO(AlunoTurmaProgramaDto? turmaPrograma)
        {
            return new RespostaTurmaProgramaAtendimentoNAAPADto
            {
                dreUe = turmaPrograma?.DreUe,
                turma = turmaPrograma?.Turma,
                componenteCurricular = turmaPrograma?.ComponenteCurricular
            };
        }

        private AtendimentoNAAPASecaoQuestaoDto MapearDTO(long questaoId, long respostaId, List<RespostaTurmaProgramaAtendimentoNAAPADto> novasTurmasPrograma)
        {
            return new AtendimentoNAAPASecaoQuestaoDto()
            {
                QuestaoId = questaoId,
                Resposta = JsonConvert.SerializeObject(novasTurmasPrograma),
                TipoQuestao = TipoQuestao.TurmasPrograma,
                RespostaEncaminhamentoId = respostaId

            };
        }
    }
}
