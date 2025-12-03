using MediatR;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtualizarEnderecoDoAtendimentoNAAPAUseCase : AbstractUseCase, IAtualizarEnderecoDoAtendimentoNAAPAUseCase
    {
        public AtualizarEnderecoDoAtendimentoNAAPAUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var encaminhamentoNAAPADto = param.ObterObjetoMensagem<AtendimentoNAAPADto>();

            var alunoEol = (await mediator.Send(new ObterAlunoEnderecoEolQuery(encaminhamentoNAAPADto.AlunoCodigo)));
            if (alunoEol.EhNulo()) return false;
            var enderecoResidencialAluno = MapearDTO(alunoEol.Endereco);

            var questaoEnderecoResidencialNAAPA = (await mediator.Send(new ObterQuestaoEnderecoAlunoEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPADto.Id ?? 0)));
            if (questaoEnderecoResidencialNAAPA.EhNulo()) return false;

            var respostaEnderecoResidencialNAAPA = questaoEnderecoResidencialNAAPA.Respostas?.FirstOrDefault();
            if (respostaEnderecoResidencialNAAPA.NaoEhNulo())
            {
                var enderecoResidencialNAAPA = JsonConvert.DeserializeObject<List<RespostaEnderecoResidencialAtendimentoNAAPADto>>(respostaEnderecoResidencialNAAPA?.Texto);
                if (enderecoResidencialAluno.EhIgual(enderecoResidencialNAAPA?.FirstOrDefault())) return false;

                var respostaEnderecoAtualizado = MapearDTO(questaoEnderecoResidencialNAAPA.QuestaoId, respostaEnderecoResidencialNAAPA?.Id ?? 0, enderecoResidencialAluno);
                return await mediator.Send(new AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(respostaEnderecoResidencialNAAPA,
                                                                                                  respostaEnderecoAtualizado));
            }
            else
                return (await mediator.Send(new RegistrarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(JsonConvert.SerializeObject(new[] { enderecoResidencialAluno }),
                                                                                                      questaoEnderecoResidencialNAAPA.QuestaoId, TipoQuestao.Endereco))) != 0;  
        }

        private RespostaEnderecoResidencialAtendimentoNAAPADto MapearDTO(EnderecoRespostaDto? endereco)
        {
            return new RespostaEnderecoResidencialAtendimentoNAAPADto
            {
                bairro = endereco?.Bairro,
                complemento = endereco?.Complemento,
                logradouro = endereco?.Logradouro,
                numero = endereco?.Nro,
                tipoLogradouro = endereco?.Tipologradouro
            };
        }

        private AtendimentoNAAPASecaoQuestaoDto MapearDTO(long questaoId, long respostaId, RespostaEnderecoResidencialAtendimentoNAAPADto novoEnderecoResidencialAluno)
        {
            return new AtendimentoNAAPASecaoQuestaoDto()
            {
                QuestaoId = questaoId,
                Resposta = JsonConvert.SerializeObject(new[] { novoEnderecoResidencialAluno }),
                TipoQuestao = TipoQuestao.Endereco,
                RespostaEncaminhamentoId = respostaId

            };
        }
    }
}
