using MediatR;
using SME.SGP.Infra.Dtos.EscolaAqui;

namespace SME.SGP.Aplicacao
{
    public class SolicitarReiniciarSenhaCommand : IRequest<RespostaSolicitarReiniciarSenhaDto>
    {
        public string Cpf { get; set; }

        public SolicitarReiniciarSenhaCommand(string cpf)
        {
            Cpf = cpf;
        }
    }
}
