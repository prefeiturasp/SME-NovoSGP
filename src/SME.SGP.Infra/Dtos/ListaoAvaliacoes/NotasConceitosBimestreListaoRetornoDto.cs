using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class NotasConceitosBimestreListaoRetornoDto
    {
        public NotasConceitosBimestreListaoRetornoDto()
        {
            Avaliacoes = new List<NotasConceitosAvaliacaoListaoRetornoDto>();
            Alunos = new List<NotasConceitosAlunoListaoRetornoDto>();
            Observacoes = new List<string>();
        }

        public List<NotasConceitosAlunoListaoRetornoDto> Alunos { get; set; }
        public List<NotasConceitosAvaliacaoListaoRetornoDto> Avaliacoes { get; set; }
        public int QtdAvaliacoesBimestrais { get; set; }
        public List<string> Observacoes { get; set; }
        public string Descricao { get; set; }
        public int Numero { get; set; }
        public long FechamentoTurmaId { get; set; }
        public SituacaoFechamento Situacao { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
    }
}
