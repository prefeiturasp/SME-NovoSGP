using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IComandosEvento
    {
        Task<IEnumerable<string>> Alterar(long id, EventoDto eventoDto);

        Task<IEnumerable<string>> Criar(EventoDto eventoDto);

        void Excluir(long[] idsEventos);
    }
}