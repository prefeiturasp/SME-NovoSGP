using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IComandosEvento
    {
        Task<IEnumerable<RetornoCopiarEventoDto>> Alterar(long id, EventoDto eventoDto);

        Task<IEnumerable<RetornoCopiarEventoDto>> Criar(EventoDto eventoDto);

        void Excluir(long[] idsEventos);

        Task GravarRecorrencia(EventoDto eventoDto, Evento evento);
    }
}