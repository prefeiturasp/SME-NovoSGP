using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class PendenciaDto
    {
        public string Tipo { get; set; }
        public string Turma { get; set; }
        public string Titulo { get; set; }
        public string Detalhe { get; set; }
        public string Bimestre { get; set; }
    }

    public class PendenciaAgrupamentoDto
    {
        public int Bimestre { get; set; }
        public int ModalidadeCodigo { get; set; }
        public string NomeTurma { get; set; }
        public IEnumerable<PendenciaDetalheDto> PendenciaDetalhes { get; set; }
    }

    public class PendenciaDetalheDto
    {
        public DateTime DataAula { get; set; }
        public bool EhReposicao { get; set; }
    }
}
