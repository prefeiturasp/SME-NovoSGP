using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IComandosEvento
    {
        Task Salvar(EventoDto eventoDto);
    }
}