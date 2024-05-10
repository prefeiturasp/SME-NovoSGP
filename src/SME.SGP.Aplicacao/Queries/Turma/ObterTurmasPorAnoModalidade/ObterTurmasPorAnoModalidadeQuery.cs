using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasPorAnoModalidadeQuery : IRequest<IEnumerable<Turma>>
    {
        public ObterTurmasPorAnoModalidadeQuery(int anoLetivo, Modalidade[] modalidades, string turmaCodigo = "")
        {
            AnoLetivo = anoLetivo;
            Modalidades = modalidades;
            TurmaCodigo = turmaCodigo;
        }

        public ObterTurmasPorAnoModalidadeQuery(int anoLetido, Modalidade modalidade, string turmaCodigo = "")
            : this(anoLetido, new Modalidade[] { modalidade }, turmaCodigo)
        {
        }

        public int AnoLetivo { get; set; }
        public Modalidade[] Modalidades { get; set; }
        public string TurmaCodigo { get; }
    }
}
