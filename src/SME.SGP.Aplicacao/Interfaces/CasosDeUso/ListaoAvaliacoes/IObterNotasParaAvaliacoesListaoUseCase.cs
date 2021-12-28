using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterNotasParaAvaliacoesListaoUseCase
    {
        Task<NotasConceitosListaoRetornoDto> Executar(ListaNotasConceitosConsultaRefatoradaDto filtro);
    }
}
