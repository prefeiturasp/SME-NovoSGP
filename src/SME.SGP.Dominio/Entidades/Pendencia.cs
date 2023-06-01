using System;
using System.Linq;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Dominio
{
    public class Pendencia : EntidadeBase
    {
        public Pendencia(TipoPendencia tipo, string titulo = "", string descricao = "", string instrucao = "", string descricaoHtml = "", long? ueId = null, long? turmaId = null)
        {
            Situacao = SituacaoPendencia.Pendente;
            Tipo = tipo;
            Titulo = titulo;
            Descricao = descricao;
            Instrucao = instrucao;
            DescricaoHtml = descricaoHtml;
            UeId = ueId;
            CriadoPor = "Sistema";
            CriadoEm = DateTime.Now;
            CriadoRF = "0";
            TurmaId = turmaId;
        }

        public Pendencia(TipoPendencia tipo, string titulo, string descricao, long? turmaId = null)
        {
            Tipo = tipo;
            Titulo = titulo;
            Descricao = descricao;
            TurmaId = turmaId;
        }

        public Pendencia()
        {
        }

        public string Descricao { get; set; }
        public SituacaoPendencia Situacao { get; set; }
        public TipoPendencia Tipo { get; set; }
        public string Titulo { get; set; }
        public string Instrucao { get; set; }
        public bool Excluido { get; set; }
        public string DescricaoHtml { get; set; }
        public long? UeId { get; set; }
        public long? TurmaId { get; set; }

        public TipoPendenciaAssunto PendenciaAssunto
        {
            get
            {
                switch (Tipo)
                {
                    case TipoPendencia.AvaliacaoSemNotaParaNenhumAluno:
                    case TipoPendencia.AulasReposicaoPendenteAprovacao:
                    case TipoPendencia.AulasSemFrequenciaNaDataDoFechamento:
                    case TipoPendencia.ResultadosFinaisAbaixoDaMedia:
                        return TipoPendenciaAssunto.PendenciaFechamento;
                    case TipoPendencia.AulaNaoLetivo:
                    case TipoPendencia.Avaliacao:
                        return TipoPendenciaAssunto.PendenciaAula;
                    case TipoPendencia.CalendarioLetivoInsuficiente:
                    case TipoPendencia.CadastroEventoPendente:
                        return TipoPendenciaAssunto.PendenciaCalendario;
                    case TipoPendencia.AusenciaDeAvaliacaoProfessor:
                    case TipoPendencia.AusenciaDeAvaliacaoCP:
                    case TipoPendencia.AusenciaFechamento:
                        return TipoPendenciaAssunto.PendenciaProfessor;
                    case TipoPendencia.AusenciaDeRegistroIndividual:
                        return TipoPendenciaAssunto.PendenciaRegistroIndividual;
                    case TipoPendencia.Devolutiva:
                        return TipoPendenciaAssunto.PendenciaDevolutiva;
                    case TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento:
                    case TipoPendencia.AlteracaoNotaFechamento:
                    case TipoPendencia.Frequencia:
                    case TipoPendencia.PlanoAula:
                    case TipoPendencia.DiarioBordo:
                    case TipoPendencia.AEE:
                    default:
                        return TipoPendenciaAssunto.Pendencia;
                }
            }
        }

        public bool EhPendenciaFechamento()
            => new TipoPendencia[] {
                TipoPendencia.AvaliacaoSemNotaParaNenhumAluno,
                TipoPendencia.AulasReposicaoPendenteAprovacao,
                TipoPendencia.AulasSemPlanoAulaNaDataDoFechamento,
                TipoPendencia.AulasSemFrequenciaNaDataDoFechamento,
                TipoPendencia.ResultadosFinaisAbaixoDaMedia,
                TipoPendencia.AlteracaoNotaFechamento
            }.Contains(Tipo);

        public bool EhPendenciaAula()
            => new TipoPendencia[] {
                TipoPendencia.Frequencia,
                TipoPendencia.PlanoAula,
                TipoPendencia.Avaliacao,
                TipoPendencia.AulaNaoLetivo
            }.Contains(Tipo);

        public bool EhPendenciaProfessor()
            => new TipoPendencia[] {
                TipoPendencia.AusenciaDeAvaliacaoProfessor,
                TipoPendencia.AusenciaDeAvaliacaoCP,
                TipoPendencia.AusenciaFechamento
            }.Contains(Tipo);

        public bool EhPendenciaCalendarioUe()
            => new TipoPendencia[] {
                TipoPendencia.CalendarioLetivoInsuficiente
            }.Contains(Tipo);

        public bool EhPendenciaDiarioBordo()
            => new TipoPendencia[] {
                TipoPendencia.DiarioBordo
            }.Contains(Tipo);

        public bool EhPendenciaCadastroEvento()
            => new TipoPendencia[] {
                TipoPendencia.CadastroEventoPendente
            }.Contains(Tipo);

        public bool EhPendenciaAusenciaAvaliacaoCP()
            => new TipoPendencia[] {
                TipoPendencia.AusenciaDeAvaliacaoCP
            }.Contains(Tipo);

        public bool EhPendenciaAusenciaAvaliacaoProfessor()
            => new TipoPendencia[] {
                TipoPendencia.AusenciaDeAvaliacaoProfessor
            }.Contains(Tipo);

        public bool EhPendenciaAusenciaDeRegistroIndividual()
            => new TipoPendencia[] {
                TipoPendencia.AusenciaDeRegistroIndividual
            }.Contains(Tipo);

        public bool EhAusenciaFechamento()
            => new TipoPendencia[] {
                TipoPendencia.AusenciaFechamento
            }.Contains(Tipo);

        public bool EhPendenciaAEE()
           => new TipoPendencia[] {
                TipoPendencia.AEE
           }.Contains(Tipo);

        public bool EhPendenciaDevolutiva()
            => new TipoPendencia[]
            {
                TipoPendencia.Devolutiva
            }.Contains(Tipo);
    }
}