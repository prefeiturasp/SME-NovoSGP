using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class FechamentoFinalSalvarDto
    {
        //public FechamentoFinalSalvarDto()
        //{
        //    Itens = new List<FechamentoFinalSalvarItemDto>();
        //}

        public bool EhRegencia { get; set; }
        public string DisciplinaId { get; set; }
        public IList<FechamentoFinalSalvarItemDto> Itens { get; set; }
        public string TurmaCodigo { get; set; }
    }
}