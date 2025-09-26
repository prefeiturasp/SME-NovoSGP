using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasPainelEducacionalTaxaAlfabetizacaoUseCase
    {
        Task<decimal> Executar(int anoLetivo, string codigoDre, string codigoUe);
    }
}
