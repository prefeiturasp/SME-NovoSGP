using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorAnoModalidadeQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasPorAnoModalidadeQuery(int anoLetivo, Modalidade[] modalidades)
        {
            AnoLetivo = anoLetivo;
            Modalidades = modalidades;
        }
        public int AnoLetivo { get; set; }
        public Modalidade[] Modalidades { get; set; }

    }
}
