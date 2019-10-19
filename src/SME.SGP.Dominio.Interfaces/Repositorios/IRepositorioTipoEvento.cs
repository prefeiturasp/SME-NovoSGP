using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Enumerados;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Interfaces.Repositorios
{
    public interface IRepositorioEventoTipo : IRepositorioBase<EventoTipo>
    {
        IList<EventoTipo> ListarTipos(EventoLocalOcorrencia eventoLocalOcorrencia, EventoLetivo eventoLetivo, string descricao);
    }
}
