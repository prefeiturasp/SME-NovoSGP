using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarEnderecoDoEncaminhamentoNAAPAUseCase : AbstractUseCase, IAtualizarEnderecoDoEncaminhamentoNAAPAUseCase
    {
        public AtualizarEnderecoDoEncaminhamentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var encaminhamentoNAAPADto = param.ObterObjetoMensagem<EncaminhamentoNAAPADto>();

            var enderecoAluno = (await mediator.Send(new ObterAlunoEnderecoEolQuery(encaminhamentoNAAPADto.AlunoCodigo)));
            if (enderecoAluno == null) return false;

            var respostaEnderecoResidencialEncaminhamentoNAAPA = (await mediator.Send(new ObterRespostaEnderecoAlunoEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPADto.Id ?? 0)));
            if (respostaEnderecoResidencialEncaminhamentoNAAPA == null) return false;
            
            var enderecoResidencialNAAPA = JsonConvert.DeserializeObject<EnderecoRespostaDto>(respostaEnderecoResidencialEncaminhamentoNAAPA.Texto);
            if (enderecoResidencialNAAPA == enderecoAluno.Endereco) return false;

            var respostaEnderecoAtualizado = new EncaminhamentoNAAPASecaoQuestaoDto() { QuestaoId = respostaEnderecoResidencialEncaminhamentoNAAPA.QuestaoEncaminhamento.Questao.Id,
                                                                                        Resposta = JsonConvert.SerializeObject(new[] { enderecoResidencialNAAPA }),
                                                                                        TipoQuestao = respostaEnderecoResidencialEncaminhamentoNAAPA.QuestaoEncaminhamento.Questao.Tipo,
                                                                                        RespostaEncaminhamentoId = respostaEnderecoResidencialEncaminhamentoNAAPA.Id

            };
            return await mediator.Send(new AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(respostaEnderecoResidencialEncaminhamentoNAAPA, 
                                                                                          respostaEnderecoAtualizado ));
        }
    }
}
