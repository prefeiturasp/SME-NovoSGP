using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class RegistroAcaoBuscaAtivaNAAPADto
    {
        public long Id { get; set; }
        private string NomeTurma { get; set; }
        private Modalidade Modalidade { get; set; }
        public string Turma => $"{Modalidade.ObterNomeCurto()}-{NomeTurma}";
        public DateTime DataRegistro { get; set; }
        public string ProcedimentoRealizado { get; set; }
        public string ConseguiuContatoResponsavel { get; set; }
        private string NomeUsuarioCriador { get; set; }
        private string RfUsuarioCriador { get; set; }
        public string Usuario => $"{NomeUsuarioCriador} ({RfUsuarioCriador})";
    }
}
