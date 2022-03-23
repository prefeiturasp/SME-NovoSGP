using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class TurmaNaoHistoricaDto
    {
        private string nomeFiltro;
        public long Id { get; set; }
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
