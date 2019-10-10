using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public interface IComandosTipoCalendarioEscolar
    {
        void Salvar(TipoCalendarioEscolarCompletoDto dto);
    }
}
