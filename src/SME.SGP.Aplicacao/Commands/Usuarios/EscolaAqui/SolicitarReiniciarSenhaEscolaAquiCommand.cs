using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui;

namespace SME.SGP.Aplicacao
{
    public class SolicitarReiniciarSenhaEscolaAquiCommand : IRequest<RespostaSolicitarReiniciarSenhaEscolaAquiDto>
    {
        public string Cpf { get; set; }

        public SolicitarReiniciarSenhaEscolaAquiCommand(string cpf)
        {
            Cpf = cpf;
        }
    }
}
