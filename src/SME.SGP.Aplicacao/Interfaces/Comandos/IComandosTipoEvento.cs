using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces.Comandos
{
    public interface IComandosEventoTipo
    {
        void Remover(IEnumerable<long> idsRemover);

        void Salvar(EventoTipoDto eventoTipoDto);
    }
}