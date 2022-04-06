using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos.PendenciaPendente
{
    public class PendenciaPerfilUsuarioDashboardDto
    {
        public long Id { get; set; }
        public long? PendenciaPerfilUsuarioId { get; set; }
        public long? PendenciaUsuarioId { get; set; }
    }
}
