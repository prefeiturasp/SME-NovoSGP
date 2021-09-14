using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoTurmaCommand : IRequest<bool>
    {
        public InserirComunicadoTurmaCommand(IEnumerable<ComunicadoTurma> turmas)
        {
            Turmas = turmas;
        }

        public IEnumerable<ComunicadoTurma> Turmas { get; set; }
    }
}
