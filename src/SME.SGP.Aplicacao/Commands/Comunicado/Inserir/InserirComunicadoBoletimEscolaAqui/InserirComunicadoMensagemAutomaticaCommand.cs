using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class InserirComunicadoMensagemAutomaticaCommand : IRequest<bool>
    {
        public InserirComunicadoMensagemAutomaticaCommand(string descricao, string titulo, int anoLetivo, string turma, int modalidade, int semestre, string aluno, Guid codigoArquivo, string urlRedirecionamentoBase, TipoRelatorio tipoRelatorio, string nomeRelatorio)
        {
            DataEnvio = DateTime.Now;
            DataExpiracao = DateTime.Now;
            Descricao = descricao;
            Titulo = titulo;
            AnoLetivo = anoLetivo;
            Turma = turma;
            AlunoEspecificado = true;
            Modalidade = modalidade;
            Semestre = semestre;
            Aluno = aluno;
            CodigoArquivo = codigoArquivo;
            UrlRedirecionamentoBase = urlRedirecionamentoBase;
            TipoRelatorio = tipoRelatorio;
            NomeRelatorio = nomeRelatorio;
        }

        public DateTime DataEnvio { get; set; }
        public DateTime DataExpiracao { get; set; }
        public string Descricao { get; set; }
        public string Titulo { get; set; }
        public int AnoLetivo { get; set; }
        public string Turma { get; set; }
        public bool AlunoEspecificado { get; set; }
        public int Modalidade { get; set; }
        public int Semestre { get; set; }
        public string Aluno { get; set; }
        public Guid CodigoArquivo { get; set; }
        public string UrlRedirecionamentoBase { get; set; }
        public TipoRelatorio TipoRelatorio { get; set; }
        public string NomeRelatorio { get; set; }

    }
}
