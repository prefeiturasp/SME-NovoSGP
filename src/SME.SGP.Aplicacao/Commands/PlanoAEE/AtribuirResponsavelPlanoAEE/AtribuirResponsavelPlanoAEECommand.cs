using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class AtribuirResponsavelPlanoAEECommand : IRequest<bool>
    {
        public AtribuirResponsavelPlanoAEECommand(PlanoAEE planoAEE, string responsavelRF, Turma turma)
        {
            PlanoAEE = planoAEE;
            ResponsavelRF = responsavelRF;
            Turma = turma;
        }

        public PlanoAEE PlanoAEE { get; set; }
        public string ResponsavelRF { get; set; }
        public Turma Turma { get; set; }
    }
}
