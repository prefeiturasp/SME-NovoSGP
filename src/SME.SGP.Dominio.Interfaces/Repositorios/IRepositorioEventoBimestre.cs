using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioEventoBimestre : IRepositorioBase<EventoBimestre>
    {
        Task ExcluiEventoBimestre(long eventoId);
    }
}
