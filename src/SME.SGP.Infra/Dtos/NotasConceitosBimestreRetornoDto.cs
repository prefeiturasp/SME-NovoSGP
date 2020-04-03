using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotasConceitosBimestreRetornoDto
    {
        public NotasConceitosBimestreRetornoDto()
        {
            Avaliacoes = new List<NotasConceitosAvaliacaoRetornoDto>();
            Alunos = new List<NotasConceitosAlunoRetornoDto>();
            Observacoes = new List<string>();
        }

        public List<NotasConceitosAlunoRetornoDto> Alunos { get; set; }
        public List<NotasConceitosAvaliacaoRetornoDto> Avaliacoes { get; set; }
        public int QtdAvaliacoesBimestrais { get; set; }
        public List<string> Observacoes { get; set; }
        public string Descricao { get; set; }
        public int Numero { get; set; }
        public bool PodeLancarNotaFinal { get; set; }
        public long FechamentoTurmaId { get; set; }
        public SituacaoFechamento Situacao { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
    }
}