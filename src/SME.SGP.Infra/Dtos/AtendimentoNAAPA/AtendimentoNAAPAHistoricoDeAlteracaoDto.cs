using SME.SGP.Dominio;
using System;
using System.ComponentModel.DataAnnotations;

namespace SME.SGP.Infra
{
    public class AtendimentoNAAPAHistoricoDeAlteracaoDto
    {
        public long Id {  get; set; }
        public string Descricao { 
            get
            {
                return $@"{ObterDescricaotipo()} {UsuarioNome} - {UsuarioLogin} em {DataHistorico.ToString("dd/MM/yyy HH:mm")}";
            }
        }
        public TipoHistoricoAlteracoesEncaminhamentoNAAPA TipoHistoricoAlteracoes { get; set; }
        public string UsuarioNome {  get; set; }
        public string UsuarioLogin {  get; set; }
        public DateTime DataHistorico { get; set; }
        public string Secao { get; set; }
        public string CamposInseridos { get; set; } 
        public string CamposAlterados { get; set; }
        public string DataAtendimento { get; set; }

        private string ObterDescricaotipo()
        {
            var complemento = ObterDescricaoImpressao() + ObterDescricaoItinerancia();

            return $@"{TipoHistoricoAlteracoes.GetAttribute<DisplayAttribute>().Name} {complemento}por";
        }

        private string ObterDescricaoImpressao()
        {
            return TipoHistoricoAlteracoes == TipoHistoricoAlteracoesEncaminhamentoNAAPA.Impressao ? "realizada " : string.Empty;
        }

        private string ObterDescricaoItinerancia()
        {
            return !string.IsNullOrEmpty(DataAtendimento) ? $@"itinerância {DataAtendimento} " : string.Empty;
        }
    }
}
