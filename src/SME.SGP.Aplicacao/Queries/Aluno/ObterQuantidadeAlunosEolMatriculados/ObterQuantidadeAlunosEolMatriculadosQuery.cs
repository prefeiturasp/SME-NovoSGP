using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeAlunosEolMatriculadosQuery : IRequest<IEnumerable<QuantidadeAlunoMatriculadoDTO>>
    {
        public ObterQuantidadeAlunosEolMatriculadosQuery(int anoLetivo, string dreCodigo, string ueCodigo, int modalidade, int anoTurma)
        {
            AnoLetivo = anoLetivo;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            Modalidade = modalidade;
            AnoTurma = anoTurma;
        }

        public int AnoLetivo { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public int Modalidade { get; set; }
        public int AnoTurma { get; set; }
    }
}
