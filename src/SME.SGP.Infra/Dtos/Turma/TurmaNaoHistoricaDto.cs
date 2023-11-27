using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class TurmaNaoHistoricaDto
    {
        private string nomeFiltroFormatado;
        public long Id { get; set; }
        public string Codigo { get; set; }
        public int CodigoModalidade { get; set; }
        public string AnoTurma { get; set; }
        public int AnoLetivo { get; set; }

        public string ModalidadeTurmaNome
        {
            get
            {
                var modalidadeEnum = (Modalidade)CodigoModalidade;
                return $"{modalidadeEnum.ShortName()} - {Nome}";
            }
        }
        public string Nome { get; set; }
        public string NomeFiltro
        {
            get => NomeFiltroFormatado();
            set => nomeFiltroFormatado = value;
        }
        private string NomeFiltroFormatado()
        {
            var modalidadeEnum = ((Modalidade)CodigoModalidade);
            return $"{modalidadeEnum.ShortName()} - {(!string.IsNullOrEmpty(nomeFiltroFormatado) ? nomeFiltroFormatado : Nome)}";
        }
    }
}
