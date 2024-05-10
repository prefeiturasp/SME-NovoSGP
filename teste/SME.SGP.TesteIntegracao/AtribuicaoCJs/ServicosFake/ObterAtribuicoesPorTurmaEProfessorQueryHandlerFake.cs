using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.AtribuicaoCJs.ServicosFake
{
    public class ObterAtribuicoesPorTurmaEProfessorQueryHandlerFake : IRequestHandler<ObterAtribuicoesPorTurmaEProfessorQuery, IEnumerable<AtribuicaoCJ>>
    {
        public ObterAtribuicoesPorTurmaEProfessorQueryHandlerFake()
        {
        }

        public async Task<IEnumerable<AtribuicaoCJ>> Handle(ObterAtribuicoesPorTurmaEProfessorQuery request, CancellationToken cancellationToken)
        {
            var atribuicoesCJ = new List<AtribuicaoCJ>(){
                new AtribuicaoCJ() { DisciplinaId = 1, ProfessorRf = "2222222", DreId ="1", UeId="1", TurmaId = "1",Modalidade = Modalidade.EducacaoInfantil, Turma = new Dominio.Turma(), Migrado = false, Substituir = true},
                new AtribuicaoCJ() { DisciplinaId = 2, ProfessorRf = "2222222", DreId ="1", UeId="1", TurmaId = "1",Modalidade = Modalidade.EducacaoInfantil, Turma = new Dominio.Turma(), Migrado = false, Substituir = true}
            };
            return atribuicoesCJ;
        }
    }
}
