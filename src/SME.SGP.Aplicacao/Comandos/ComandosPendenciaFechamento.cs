using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosPendenciaFechamento: IComandosPendenciaFechamento
    {
        private readonly IServicoPendenciaFechamento servicoPendenciaFechamento;

        public ComandosPendenciaFechamento(IServicoPendenciaFechamento servicoPendenciaFechamento)
        {
            this.servicoPendenciaFechamento = servicoPendenciaFechamento ?? throw new ArgumentNullException(nameof(servicoPendenciaFechamento));
        }

        public async Task<AuditoriaDto> Aprovar(long pendenciaId)
            => await servicoPendenciaFechamento.Aprovar(pendenciaId);
    }
}
