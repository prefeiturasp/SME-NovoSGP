using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCJQuery: IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesCJQuery(Modalidade? modalidade, string turmaCodigo, string ueCodigo, long componenteCurricular, string professorRf, bool listarComponentesPlanejamento)
        {
            Modalidade = modalidade;
            TurmaCodigo = turmaCodigo;
            UeCodigo = ueCodigo;
            ComponenteCurricular = componenteCurricular;
            ProfessorRf = professorRf;
            ListarComponentesPlanejamento = listarComponentesPlanejamento;
        }

        public Modalidade? Modalidade { get; set; }
        public string TurmaCodigo { get; set; }
        public string UeCodigo { get; set; }
        public long ComponenteCurricular { get; set; }
        public string ProfessorRf { get; set; }
        public bool ListarComponentesPlanejamento { get; set; }
    }
}
