using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IConsultasNotasConceitos
    {
        Task<NotasConceitosRetornoDto> ListarNotasConceitos(string turmaId, int? bimestre, int anoLetivo, string disciplinaCodigo, Modalidade modalidade);

        TipoNota ObterNotaTipo(long turmaId, int anoLetivo);

        double ObterValorArredondado(long atividadeAvaliativaId, double nota);
    }
}