using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosAula
    {
        Task<string> Alterar(AulaDto dto, long id);

        Task<string> Excluir(long id, string disciplinaNome, RecorrenciaAula recorrencia);

        Task<string> Inserir(AulaDto dto);
    }
}