using System;
using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class RegistroAcaoBuscaAtivaCriancaEstudanteAusenteDto
    {
        public long Id { get; set; }
        public DateTime DataRegistro  { get; set; }
        public string Usuario { get; set; }
    }
}