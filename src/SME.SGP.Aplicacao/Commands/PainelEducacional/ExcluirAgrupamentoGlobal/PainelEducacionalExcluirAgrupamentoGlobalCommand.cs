using MediatR;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoGlobal
{
    public class PainelEducacionalExcluirAgrupamentoGlobalCommand : IRequest<bool>
    {

        public PainelEducacionalExcluirAgrupamentoGlobalCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
        public int AnoLetivo { get; set; }
    }
}
