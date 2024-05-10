
using System;

namespace SME.SGP.Infra
{
    public class PendenciaDiarioBordoDescricaoDto
    {
        public DateTime DataAula { get; set; }
        public string ComponenteCurricular { get; set; }
        public long PendenciaId { get; set; }
        public int Bimestre { get; set; }
        public int ModalidadeCodigo { get; set; }
        public string NomeTurma { get; set; }
        public bool EhReposicao { get; set; }
    }
}
