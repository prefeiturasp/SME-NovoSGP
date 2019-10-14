using SME.SGP.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao.Interfaces.Comandos
{
    public interface IComandosPeriodoEscolar
    {
        void Salvar(PeriodoEscolarListaDto periodosDto);
    }
}
