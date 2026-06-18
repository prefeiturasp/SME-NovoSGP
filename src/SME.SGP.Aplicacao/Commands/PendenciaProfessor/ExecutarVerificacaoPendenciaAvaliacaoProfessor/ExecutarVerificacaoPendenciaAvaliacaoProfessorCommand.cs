using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExecutarVerificacaoPendenciaAvaliacaoProfessorCommand : IRequest<bool>
    {
        public ExecutarVerificacaoPendenciaAvaliacaoProfessorCommand(int diasParaGeracaoDePendencia)
        {
            DiasParaGeracaoDePendencia = diasParaGeracaoDePendencia;
        }

        public int DiasParaGeracaoDePendencia { get; set; }
    }
}
