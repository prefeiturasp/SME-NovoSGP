using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.EscolaAqui;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPorCpfQuery : IRequest<UsuarioEscolaAquiDto>
    {
        public string Cpf { get; set; }

        public ObterUsuarioPorCpfQuery(string cpf)
        {
            Cpf = cpf;
        }
    }
}
