using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoAlunoCommand : IRequest<bool>
    {
        public InserirComunicadoAlunoCommand(IEnumerable<ComunicadoAluno> alunos)
        {
            Alunos = alunos;
        }

        public IEnumerable<ComunicadoAluno> Alunos { get; set; }
    }
}
