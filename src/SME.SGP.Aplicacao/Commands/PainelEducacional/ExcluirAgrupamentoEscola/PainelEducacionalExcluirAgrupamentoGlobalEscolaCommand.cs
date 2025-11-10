using MediatR;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.ExcluirAgrupamentoEscola
{
    public class PainelEducacionalExcluirAgrupamentoGlobalEscolaCommand : IRequest<bool>
    {
        public PainelEducacionalExcluirAgrupamentoGlobalEscolaCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
        public int AnoLetivo { get; set; }
    }
}
