using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IComandosEventoTipo
    {
        void Alterar(EventoTipoInclusaoDto eventoTipoDto, long idEvento);

        void Remover(IEnumerable<long> idsRemover);

        void Salvar(EventoTipoInclusaoDto eventoTipoDto);
    }
}