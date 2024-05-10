using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessoresTitularesDaTurmaCompletosQuery : IRequest<IEnumerable<ProfessorTitularDisciplinaEol>>
    {
        public ObterProfessoresTitularesDaTurmaCompletosQuery(string codigoTurma, bool realizarAgrupamento = false)
        {
            CodigoTurma = codigoTurma;
            RealizarAgrupamento = realizarAgrupamento;
        }

        public string CodigoTurma { get; set; }
        public bool RealizarAgrupamento { get; set; }
    }
}
