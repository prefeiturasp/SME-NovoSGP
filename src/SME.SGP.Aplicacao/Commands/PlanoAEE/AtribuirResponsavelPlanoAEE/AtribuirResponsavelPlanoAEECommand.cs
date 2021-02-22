using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelPlanoAEECommand : IRequest<bool>
    {
        public AtribuirResponsavelPlanoAEECommand(long planoAEEId, string responsavelRF)
        {
            PlanoAEEId = planoAEEId;
            ResponsavelRF = responsavelRF;
        }

        public long PlanoAEEId { get; set; }
        public string ResponsavelRF { get; set; }
    }
}
