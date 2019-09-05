using SME.SGP.Aplicacao.Integracoes.Respostas;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Integracoes
{
    public interface IServicoEOL
    {
        IEnumerable<ProfessorTurmaReposta> ObterListaTurmasPorProfessor(string codigoRf);
    }
}