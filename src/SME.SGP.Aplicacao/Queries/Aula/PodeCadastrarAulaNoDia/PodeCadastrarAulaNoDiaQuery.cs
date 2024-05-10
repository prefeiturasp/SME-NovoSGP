using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class PodeCadastrarAulaNoDiaQuery: IRequest<bool>
    {
        public PodeCadastrarAulaNoDiaQuery(DateTime dataAula, string turmaCodigo, long[] componentesCurriculares, TipoAula tipoAula, string professorRf = null)
        {
            DataAula = dataAula;
            TurmaCodigo = turmaCodigo;
            ComponentesCurriculares = componentesCurriculares;
            ProfessorRf = professorRf;
            TipoAula = tipoAula;
        }

        public DateTime DataAula { get; set; }
        public string TurmaCodigo { get; set; }
        public long[] ComponentesCurriculares { get; set; }
        public string ProfessorRf { get; set; }
        public TipoAula TipoAula { get; set; }
    }
}
