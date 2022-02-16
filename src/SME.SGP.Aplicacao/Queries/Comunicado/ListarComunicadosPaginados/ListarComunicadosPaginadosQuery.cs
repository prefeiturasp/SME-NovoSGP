using MediatR;
using SME.SGP.Dto;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ListarComunicadosPaginadosQuery : IRequest<PaginacaoResultadoDto<ComunicadoAlunoReduzidoDto>>
    {
        public ListarComunicadosPaginadosQuery(string dreCodigo, string ueCodigo, string turmaCodigo, string alunoCodigo)
        {
            DRECodigo = dreCodigo;
            UECodigo = ueCodigo;
            TurmaCodigo = turmaCodigo;
            AlunoCodigo = alunoCodigo;
        }

        public string DRECodigo { get; set; }
        public string UECodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public string AlunoCodigo { get; set; }
    }
}
