using MediatR;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoMensal
{
    public class PainelEducacionalExcluirAgrupamentoMensalCommand : IRequest<bool>
    {
        public PainelEducacionalExcluirAgrupamentoMensalCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
    }
}
