using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmasCodigoQuery : IRequest<IEnumerable<DisciplinaDto>>
    {
        public ObterComponentesCurricularesPorTurmasCodigoQuery(string[] turmasCodigo, Guid perfilAtual, string loginAtual, bool temEnsinoEspecial)
        {
            TurmasCodigo = turmasCodigo;
            PerfilAtual = perfilAtual;
            LoginAtual = loginAtual;
            TemEnsinoEspecial = temEnsinoEspecial;
        }

        public string[] TurmasCodigo { get; set; }
        public Guid PerfilAtual { get; set; }
        public string LoginAtual { get; set; }
        public bool TemEnsinoEspecial { get; set; }
    }
}
