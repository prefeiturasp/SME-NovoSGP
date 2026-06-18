using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.MapeamentoEstudantes.ServicosFakes
{
    public class ObterAlunoEnderecoEolQueryAlunoMigranteFake : IRequestHandler<ObterAlunoEnderecoEolQuery, AlunoEnderecoRespostaDto>
    {
        public ObterAlunoEnderecoEolQueryAlunoMigranteFake()
        {}

        public Task<AlunoEnderecoRespostaDto> Handle(ObterAlunoEnderecoEolQuery request, CancellationToken cancellationToken)
        => Task.Run(() => new AlunoEnderecoRespostaDto() { Nacionalidade = "Brasil" });
    }
}
