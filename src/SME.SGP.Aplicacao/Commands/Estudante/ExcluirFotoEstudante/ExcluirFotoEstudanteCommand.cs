using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFotoEstudanteCommand : IRequest<bool>
    {
        public ExcluirFotoEstudanteCommand(string alunoCodigo)
        {
            AlunoCodigo = alunoCodigo;
        }

        public string AlunoCodigo { get; set; }
    }

}
