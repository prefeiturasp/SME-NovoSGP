using MediatR;

namespace SME.SGP.Aplicacao.Commands.PainelEducacional.Reclassificacao.ExcluirReclassificacaoAnual
{
    public class PainelEducacionalExcluirReclassificacaoAnualCommand : IRequest<bool>
    {
        public PainelEducacionalExcluirReclassificacaoAnualCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
    }
}
