using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Org.BouncyCastle.Math.EC.Rfc7748;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarTurmasProgramaDoEncaminhamentoNAAPAUseCase : AbstractUseCase, IAtualizarTurmasProgramaDoEncaminhamentoNAAPAUseCase
    {
        public AtualizarTurmasProgramaDoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var encaminhamentoNAAPADto = param.ObterObjetoMensagem<EncaminhamentoNAAPADto>();
            var turma = (await mediator.Send(new ObterTurmaPorIdQuery(encaminhamentoNAAPADto.TurmaId)));

            var turmasProgramaAluno = (await mediator.Send(new ObterTurmasProgramaAlunoQuery(encaminhamentoNAAPADto.AlunoCodigo, turma.AnoLetivo, true)))
                                        .Select(turmaPrograma => MapearDTO(turmaPrograma)).ToList();

            var questaoTurmasProgramaNAAPA = (await mediator.Send(new ObterQuestaoTurmasProgramaEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPADto.Id ?? 0)));
            if (questaoTurmasProgramaNAAPA.EhNulo()) return false;

            var respostaTurmasProgramaNAAPA = questaoTurmasProgramaNAAPA.Respostas?.FirstOrDefault();
            if (respostaTurmasProgramaNAAPA.NaoEhNulo())
            {
                var turmasProgramaNaapa = JsonConvert.DeserializeObject<List<RespostaTurmaProgramaEncaminhamentoNAAPADto>>(respostaTurmasProgramaNAAPA.Texto);
                if (turmasProgramaAluno.Count == turmasProgramaNaapa?.Count &&
                    turmasProgramaAluno.All(turmaProgramaAluno => turmasProgramaNaapa.NaoEhNulo() && turmasProgramaNaapa.Any(x => x.Equals(turmaProgramaAluno)))) 
                    return false;

                var respostaEnderecoAtualizado = MapearDTO(questaoTurmasProgramaNAAPA.QuestaoId, respostaTurmasProgramaNAAPA.Id, turmasProgramaAluno);
                return await mediator.Send(new AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(respostaTurmasProgramaNAAPA,
                                                                                                  respostaEnderecoAtualizado));
            }
            else
                return (await mediator.Send(new RegistrarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(JsonConvert.SerializeObject(turmasProgramaAluno),
                                                                                                      questaoTurmasProgramaNAAPA.QuestaoId, TipoQuestao.TurmasPrograma))) != 0;  
        }

        private RespostaTurmaProgramaEncaminhamentoNAAPADto MapearDTO(AlunoTurmaProgramaDto? turmaPrograma)
        {
            return new RespostaTurmaProgramaEncaminhamentoNAAPADto
            {
                dreUe = turmaPrograma?.DreUe,
                turma = turmaPrograma?.Turma,
                componenteCurricular = turmaPrograma?.ComponenteCurricular
            };
        }

        private EncaminhamentoNAAPASecaoQuestaoDto MapearDTO(long questaoId, long respostaId, List<RespostaTurmaProgramaEncaminhamentoNAAPADto> novasTurmasPrograma)
        {
            return new EncaminhamentoNAAPASecaoQuestaoDto()
            {
                QuestaoId = questaoId,
                Resposta = JsonConvert.SerializeObject(novasTurmasPrograma),
                TipoQuestao = TipoQuestao.TurmasPrograma,
                RespostaEncaminhamentoId = respostaId

            };
        }
    }
}
