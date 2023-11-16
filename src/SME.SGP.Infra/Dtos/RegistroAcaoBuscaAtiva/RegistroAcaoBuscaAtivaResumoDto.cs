using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class RegistroAcaoBuscaAtivaResumoDto
    {
        public long Id { get; set; }
        public string Ue { get; set; }
        public string UeNome { get; set; }
        public TipoEscola TipoEscola { get; set; }
        public string NomeAluno { get; set; }
        public string CodigoAluno { get; set; }
        public DateTime? DataRegistro  { get; set; }
    }
}