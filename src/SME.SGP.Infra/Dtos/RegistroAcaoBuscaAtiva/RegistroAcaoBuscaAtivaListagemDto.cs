using SME.SGP.Dominio;
using System;

namespace SME.SGP.Infra
{
    public class RegistroAcaoBuscaAtivaListagemDto
    {
        public long Id { get; set; }
        private string NomeTurma { get; set; }
        private Modalidade Modalidade { get; set; }
        public string Turma => $"{Modalidade.ObterNomeCurto()}-{NomeTurma}";
        private string NomeAluno { get; set; }
        private string CodigoAluno { get; set; }
        public string CriancaEstudante => $"{NomeAluno} ({CodigoAluno})";
        public DateTime DataRegistro  { get; set; }
        public string ProcedimentoRealizado { get; set; }
        public string ConseguiuContatoResponsavel { get; set; }
        private string NomeUsuarioCriador { get; set; }
        private DateTime DataCriacao { get; set; }
        public string InseridoPor => $"{NomeUsuarioCriador} em {DataCriacao.ToString("dd/MM/yyyy")} as {DataCriacao.ToString("HH:mm")}";
        public string DescMotivoAusencia { get; set; }
        
        public string Ue { get; set; }
        public string UeNome { get; set; }
        public TipoEscola TipoEscola { get; set; }
    }
}