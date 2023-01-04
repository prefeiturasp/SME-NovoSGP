using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dto;
using SME.SGP.Infra;

namespace SME.SGP.TesteIntegracao.EncaminhamentoNAAPA.ServicosFake
{
    public class ObterAlunoEnderecoEolQueryHandlerFake : IRequestHandler<ObterAlunoEnderecoEolQuery, AlunoEnderecoRespostaDto>
    {

        public async Task<AlunoEnderecoRespostaDto> Handle(ObterAlunoEnderecoEolQuery request, CancellationToken cancellationToken)
        {
            return new AlunoEnderecoRespostaDto {Endereco = new EnderecoRespostaDto { Bairro = "Centro", 
                                                                                      CEP = 99880000, 
                                                                                      Complemento = "Casa",
                                                                                      Logradouro = "Rua das maçãs",
                                                                                      NomeMunicipio = "Macieira",
                                                                                      Nro = "142",
                                                                                      SiglaUF = "SC",
                                                                                      Tipologradouro = "Rua"
            }};
        }
    }
}