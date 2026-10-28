using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosMatriculadosPorAnoLetivoECCEolQuery : IRequest<IEnumerable<AlunosMatriculadosEolDto>>
    {
        public string UeCodigo { get; set; }
        public int Ano { get; set; }
        public string DreCodigo { get; set; }
        public int[] ComponentesCurriculares { get; set; }

        public ObterAlunosMatriculadosPorAnoLetivoECCEolQuery(int ano, string dreCodigo, string ueCodigo, int[] componentesCurriculares)
        {
            Ano = ano;
            DreCodigo = dreCodigo;
            UeCodigo = ueCodigo;
            ComponentesCurriculares = componentesCurriculares;
        }
    }
}
