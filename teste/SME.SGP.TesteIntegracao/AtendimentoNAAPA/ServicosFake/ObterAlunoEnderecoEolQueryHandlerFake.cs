using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.AtendimentoNAAPA.ServicosFake
{
    public class ObterAlunoEnderecoEolQueryHandlerFake : IRequestHandler<ObterAlunoEnderecoEolQuery, AlunoEnderecoRespostaDto>
    {

        public async Task<AlunoEnderecoRespostaDto> Handle(ObterAlunoEnderecoEolQuery request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(new AlunoEnderecoRespostaDto
            {
                Endereco = new EnderecoRespostaDto
                {
                    Bairro = "Centro",
                    CEP = 99880000,
                    Complemento = "Casa",
                    Logradouro = "Rua das maçãs",
                    NomeMunicipio = "Macieira",
                    Nro = "142",
                    SiglaUF = "SC",
                    Tipologradouro = "Rua"
                }
            });
        }
    }
}