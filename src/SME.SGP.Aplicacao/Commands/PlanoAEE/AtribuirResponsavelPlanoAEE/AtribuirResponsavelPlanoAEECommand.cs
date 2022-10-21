using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelPlanoAEECommand : IRequest<bool>
    {
        public AtribuirResponsavelPlanoAEECommand(long planoAEEId, string responsavelRF, Turma turma)
        {
            PlanoAEEId = planoAEEId;
            ResponsavelRF = responsavelRF;
            Turma = turma;
        }

        public long PlanoAEEId { get; set; }
        public string ResponsavelRF { get; set; }
        public Turma Turma { get; set; }
    }
}
