using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterMiniaturaFotoEstudantePorAlunoCodigoQuery : IRequest<MiniaturaFotoDto>
    {

        public ObterMiniaturaFotoEstudantePorAlunoCodigoQuery(string alunoCodigo)
        {
            AlunoCodigo = alunoCodigo;
        }
        public string AlunoCodigo { get; set; }
    }
}