using Nest;
using SME.SGP.Dominio;
using System;
using System.Linq;

namespace SME.SGP.Infra.ElasticSearch.Dtos
{
    [ElasticsearchType(RelationName = "alunonaturma")]
    public class AlunoNaTurmaElasticDTO : DocumentoElasticTurma
    {
        public int CodigoComponenteCurricular { get; set; }

        [Number(Name = "codigoaluno")]
        public int CodigoAluno { get; set; }
        [Text(Name = "nomealuno")]
        public string NomeAluno { get; set; }
        [Date(Name = "datanascimento", Format = "MMddyyyy")]
        public DateTime DataNascimento { get; set; }
        [Text(Name = "nomesocialaluno")]
        public string NomeSocialAluno { get; set; }
        [Number(Name = "codigosituacaomatricula")]
        public int CodigoSituacaoMatricula { get; set; }
        [Text(Name = "situacaomatricula")]
        public string SituacaoMatricula { get; set; }
        [Date(Name = "datasituacao", Format = "MMddyyyy")]
        public DateTime DataSituacao { get; set; }
        [Date(Name = "datamatricula", Format = "MMddyyyy")]
        public DateTime DataMatricula { get; set; }
        [Text(Name = "numeroalunochamada")]
        public string NumeroAlunoChamada { get; set; }
        [Number(Name = "possuideficiencia")]
        public int PossuiDeficiencia { get; set; }
        [Number(Name = "codigoturma")]
        public int TurmaCodigo { get; set; }
        public bool Transferencia_Interna { get; set; }
        public bool Remanejado { get; set; }
        public string EscolaTransferencia { get; set; }
        public string TurmaTransferencia { get; set; }
        public string TurmaRemanejamento { get; set; }
        public char? ParecerConclusivo { get; set; }
        [Text(Name = "nomeresponsavel")]
        public string NomeResponsavel { get; set; }
        [Number(Name = "tiporesponsavel")]
        public int? TipoResponsavel { get; set; }
        [Text(Name = "celularresponsavel")]
        public string CelularResponsavel { get; set; }
        [Date(Name = "dataatualizacaocontato", Format = "MMddyyyy")]
        public DateTime? DataAtualizacaoContato { get; set; }
        [Number(Name = "codigomatricula")]
        public long CodigoMatricula { get; set; }
        [Number(Name = "sequencia")]
        public int Sequencia { get; set; }
        [Number(Name = "tipoturma")]
        public int TipoTurma { get; set; }
        [Text(Name = "codigodre")]
        public string CodigoDre { get; set; }

        private string[] SituacoesAtivas => new string[]{
                                                           SituacaoMatriculaAluno.Ativo.Name(),
                                                           SituacaoMatriculaAluno.Concluido.Name(),
                                                           SituacaoMatriculaAluno.PendenteRematricula.Name(),
                                                           SituacaoMatriculaAluno.Rematriculado.Name(),
                                                           SituacaoMatriculaAluno.SemContinuidade.Name()};

        public bool Ativo => SituacoesAtivas.Contains(SituacaoMatricula);
        public bool Inativo { get { return !Ativo; } }

        public string ObterNomeFinal()
        {
            if (string.IsNullOrEmpty(NomeSocialAluno))
                return NomeAluno;
            else return NomeSocialAluno;
        }
    }
}
