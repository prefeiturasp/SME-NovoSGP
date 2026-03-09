using SME.SGP.Dominio.Enumerados;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class PlanoAEE : EntidadeBase
    {
        public PlanoAEE()
        {
            Questoes = new List<PlanoAEEQuestao>();
        }

        public Turma Turma { get; set; }
        public long TurmaId { get; set; }
        public int AlunoNumero { get; set; }
        public string AlunoCodigo { get; set; }
        public string AlunoNome { get; set; }
        public SituacaoPlanoAEE Situacao { get; set; }
        public string ParecerCoordenacao { get; set; }
        public string ParecerPAAI { get; set; }
        public long? ResponsavelPaaiId { get; set; }
        public long ResponsavelId { get; set; }
        public List<PlanoAEEQuestao> Questoes { get; set; }

        public void EncerrarPlanoAEE() {
            Situacao = SituacaoPlanoAEE.ParecerCP;
        }

        public void EncerramentoManualPlanoAEE()
        {
            Situacao = SituacaoPlanoAEE.Encerrado;
        }

        public bool SituacaoPodeDevolverPlanoAEE()
            => Situacao == SituacaoPlanoAEE.ParecerCP
            || Situacao == SituacaoPlanoAEE.ParecerPAAI
            || Situacao == SituacaoPlanoAEE.AtribuicaoPAAI;

        public bool EhSituacaoExpiradoValidado()
            => Situacao.EhUmDosValores(SituacaoPlanoAEE.Expirado, 
                                        SituacaoPlanoAEE.Validado);

        public SituacaoPlanoAEE ObterSituacaoAoRemoverResponsavelPAAI()
            => EhSituacaoExpiradoValidado()
                ? Situacao
                : SituacaoPlanoAEE.AtribuicaoPAAI;

        public SituacaoPlanoAEE ObterSituacaoAoAtribuirResponsavelPAAI()
            => EhSituacaoExpiradoValidado()
                ? Situacao
                : SituacaoPlanoAEE.ParecerPAAI;
    }
}
