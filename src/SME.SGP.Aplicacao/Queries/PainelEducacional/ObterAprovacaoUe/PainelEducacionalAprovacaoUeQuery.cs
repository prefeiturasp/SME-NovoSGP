using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterAprovacaoUe
{
    public class PainelEducacionalAprovacaoUeQuery : IRequest<PaginacaoResultadoDto<PainelEducacionalAprovacaoUeDto>>
    {
        public PainelEducacionalAprovacaoUeQuery(int anoLetivo, string codigoUe, int modalidadeId)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
            ModalidadeId = modalidadeId;
        }

        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
        public int ModalidadeId { get; set; }
    }
}
