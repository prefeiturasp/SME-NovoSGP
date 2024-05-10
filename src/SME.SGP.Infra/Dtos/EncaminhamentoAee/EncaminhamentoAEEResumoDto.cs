﻿using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class EncaminhamentoAEEResumoDto
    {
        public long Id { get; set; }
        public int Numero { get; set; }
        public string Nome { get; set; }
        public string Turma { get; set; }
        public string Situacao { get; set; }
        public SituacaoAEE SituacaoTipo { get; set; }
        public string Responsavel { get; set; }
        public bool EhAtendidoAEE { get; set; }
        public bool EhMatriculadoTurmaPAP { get; set; }
        public string Ue { get; set; }
    }
}
