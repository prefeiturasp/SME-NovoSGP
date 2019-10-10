using System.Collections.Generic;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;

namespace SME.SGP.Aplicacao
{
    public class ConsultasTipoCalendarioEscolar : IConsultasTipoCalendarioEscolar
    {
        private readonly IRepositorioTipoCalendarioEscolar repositorio;

        public ConsultasTipoCalendarioEscolar(IRepositorioTipoCalendarioEscolar repositorio)
        {
            this.repositorio = repositorio ?? throw new System.ArgumentNullException(nameof(repositorio));
        }
        public IEnumerable<TipoCalendarioEscolarDto> Listar()
        {
            return this.repositorio.ObterTiposCalendarioEscolar();
        }
    }
}
