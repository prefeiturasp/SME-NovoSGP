using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ConsultaDisciplina.ServicosFake
{
    public class ObterTurmaComUeEDrePorCodigoQueryHandlerFake : IRequestHandler<ObterTurmaComUeEDrePorCodigoQuery, SME.SGP.Dominio.Turma>
    {
        public ObterTurmaComUeEDrePorCodigoQueryHandlerFake() { }
        public async Task<SME.SGP.Dominio.Turma> Handle(ObterTurmaComUeEDrePorCodigoQuery request, CancellationToken cancellationToken)
        {
            return new SME.SGP.Dominio.Turma() { CodigoTurma = "1", ModalidadeCodigo = Modalidade.EducacaoInfantil };
        }
    }
}
