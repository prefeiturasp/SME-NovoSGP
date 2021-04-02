using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorTurmasCodigoQuery : IRequest<IEnumerable<DisciplinaDto>>
    {
        public ObterComponentesCurricularesPorTurmasCodigoQuery(string[] turmasCodigo, Guid perfilAtual, string loginAtual, bool temEnsinoEspecial, int turnoParaComponentesCurriculares)
        {
            TurmasCodigo = turmasCodigo;
            PerfilAtual = perfilAtual;
            LoginAtual = loginAtual;
            TemEnsinoEspecial = temEnsinoEspecial;
            TurnoParaComponentesCurriculares = turnoParaComponentesCurriculares;
        }

        public string[] TurmasCodigo { get; set; }
        public Guid PerfilAtual { get; set; }
        public string LoginAtual { get; set; }
        public bool TemEnsinoEspecial { get; set; }
        public Modalidade? TurmaModalidade { get; set; }
        public int TurmaAno { get; set; }
        public int TurnoParaComponentesCurriculares { get; set; }
    }
}
