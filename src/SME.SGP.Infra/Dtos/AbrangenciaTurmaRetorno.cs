using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Dto
{
    public class AbrangenciaTurmaRetorno
    {
        private string nomeFiltro;
        public string Ano { get; set; }
        public int AnoLetivo { get; set; }
        public string Codigo { get; set; }
        public int CodigoModalidade { get; set; }
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
            set { nomeFiltro = value; }
        }

        public int Semestre { get; set; }
        public bool EnsinoEspecial { get; set; }
        public long Id { get; set; }

        public int TipoTurma { get; set; }

        private string nomeFiltro { get; set; }
        public string NomeFiltro
        {
            get => NomeFiltroFormatado();
            set => nomeFiltro = value;
        }

        public string NomeFiltroFormatado()
        {
            var modalidadeEnum = ((Modalidade)CodigoModalidade);
            if (nomeFiltro != null)
                return $"{modalidadeEnum.ShortName()} - {nomeFiltro}";
            else
                return $"{modalidadeEnum.ShortName()} - {Nome}";
        }

    }
}