using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            var alunoEol = (await mediator.Send(new ObterAlunoEnderecoEolQuery(encaminhamentoNAAPADto.AlunoCodigo)));
            if (alunoEol == null) return false;
            var enderecoResidencialAluno = MapearDTO(alunoEol.Endereco);

            var respostasEnderecoResidencialNAAPA = (await mediator.Send(new ObterRespostaEnderecoAlunoEncaminhamentoNAAPAPorIdQuery(encaminhamentoNAAPADto.Id ?? 0)));
            if (respostasEnderecoResidencialNAAPA == null) return false;
            
            var enderecosResidenciaisNAAPA = JsonConvert.DeserializeObject<List<RespostaEnderecoResidencialEncaminhamentoNAAPADto>>(respostasEnderecoResidencialNAAPA.Texto);
            var enderecoResidencialNAAPA = enderecosResidenciaisNAAPA?.FirstOrDefault();
            if (enderecoResidencialAluno.Equals(enderecoResidencialNAAPA)) return false;

            var respostaEnderecoAtualizado = MapearDTO(respostasEnderecoResidencialNAAPA, enderecoResidencialAluno);
            return await mediator.Send(new AlterarEncaminhamentoNAAPASecaoQuestaoRespostaCommand(respostasEnderecoResidencialNAAPA,
                                                                                              respostaEnderecoAtualizado));
        }

        private RespostaEnderecoResidencialEncaminhamentoNAAPADto MapearDTO(EnderecoRespostaDto? endereco)
        {
            return new RespostaEnderecoResidencialEncaminhamentoNAAPADto
            {
                bairro = endereco?.Bairro,
                complemento = endereco?.Complemento,
                logradouro = endereco?.Logradouro,
                numero = endereco?.Nro,
                tipoLogradouro = endereco?.Tipologradouro
            };
        }

        private EncaminhamentoNAAPASecaoQuestaoDto MapearDTO(RespostaEncaminhamentoNAAPA respostasEnderecoResidencialNAAPA, RespostaEnderecoResidencialEncaminhamentoNAAPADto novoEnderecoResidencialAluno)
        {
            return new EncaminhamentoNAAPASecaoQuestaoDto()
            {
                QuestaoId = respostasEnderecoResidencialNAAPA.QuestaoEncaminhamento.Questao.Id,
                Resposta = JsonConvert.SerializeObject(new[] { novoEnderecoResidencialAluno }),
                TipoQuestao = respostasEnderecoResidencialNAAPA.QuestaoEncaminhamento.Questao.Tipo,
                RespostaEncaminhamentoId = respostasEnderecoResidencialNAAPA.Id

            };
        }
    }
}
