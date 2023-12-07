using System;
using System.Xml.Linq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using StackExchange.Redis;

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
        public string ContatoEfetuadoResponsavel { get; set; }
        public string CriancaRetornouEscolaAposContato { get; set; }
        private string NomeUsuarioCriador { get; set; }
        private DateTime DataCriacao { get; set; }
        public string InseridoPor => $"{NomeUsuarioCriador} em {DataCriacao.ToString("dd/MM/yyyy")} as {DataCriacao.ToString("HH:mm")}";
    }
}