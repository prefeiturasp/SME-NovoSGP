using System;
using System.Linq;

namespace SME.SGP.Dominio
{
    public class Pendencia : EntidadeBase
    {
        public Pendencia(TipoPendencia tipo, string titulo = "", string descricao = "", string instrucao = "", string descricaoHtml = "", long? ueId = null)
        {
            Situacao = SituacaoPendencia.Pendente;
            Tipo = tipo;
            Titulo = titulo;
            Descricao = descricao;
            Instrucao = instrucao;
            DescricaoHtml = descricaoHtml;
            UeId = ueId;
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