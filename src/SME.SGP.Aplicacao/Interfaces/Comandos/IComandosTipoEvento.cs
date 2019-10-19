using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Interfaces.Comandos
{
    public interface IComandosEventoTipo
    {
        void Salvar(EventoTipoDto eventoTipoDto);
        void Remover(long Codigo);
    }
}
