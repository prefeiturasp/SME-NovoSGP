using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class CargaPendenciasQuantidadeDiasQuantidadeAulasCommand : IRequest<bool>
    {
        public CargaPendenciasQuantidadeDiasQuantidadeAulasCommand(CargaAulasDiasPendenciaDto carga)
        {
            Carga = carga;
        }

        public CargaAulasDiasPendenciaDto Carga { get; set; }
    }
}