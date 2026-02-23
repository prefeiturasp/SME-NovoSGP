using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra.Dtos.PainelEducacional.InformacoesEducacionais;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional
{
    public interface IConsultasInformacoesEducacionaisUseCase
    {
        Task<InformacoesEducacionaisRetornoDto> ObterInformacoesEducacionais(FiltroInformacoesEducacionais filtro);
    }
}
