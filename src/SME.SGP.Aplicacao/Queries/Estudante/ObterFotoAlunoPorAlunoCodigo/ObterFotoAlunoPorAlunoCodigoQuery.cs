using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterFotoAlunoPorAlunoCodigoQuery : IRequest<AlunoFoto>
    {
        public ObterFotoAlunoPorAlunoCodigoQuery(string codigoAluno)
        {
            CodigoAluno = codigoAluno;
        }

        public string CodigoAluno { get; }
    }
}
