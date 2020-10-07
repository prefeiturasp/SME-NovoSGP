using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorCpfQuery : IRequest<UsuarioEscolaAquiDto>
    {
        public string CodigoDre { get; set; }
        public long CodigoUe { get; set; }
        public string Cpf { get; set; }

        public ObterUsuarioPorCpfQuery(string codigoDre, long codigoUe, string cpf)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
            Cpf = cpf;
        }
    }
}
