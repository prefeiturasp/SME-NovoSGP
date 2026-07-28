using Dapper.Contrib.Extensions;
using System;

namespace SME.SGP.Dominio
{
    public class Ue
    {
        public Ue()
        {
            DataAtualizacao = DateTimeExtension.HorarioBrasilia();
        }
        public string CodigoUe { get; set; }
        public DateTime DataAtualizacao { get; set; }
        [Computed]
        public Dre Dre { get; set; }
        public long DreId { get; set; }
        [Key]
        public long Id { get; set; }
        public string Nome { get; set; }
        public TipoEscola TipoEscola { get; set; }

        public void AdicionarDre(Dre dre)
        {
            if (dre.NaoEhNulo())
            {
                Dre = dre;
                DreId = dre.Id;
            }
        }

        public bool EhUnidadeInfantil()
        {
            return TipoEscola == TipoEscola.EMEI || TipoEscola == TipoEscola.CEUEMEI
                || TipoEscola == TipoEscola.CEMEI || TipoEscola == TipoEscola.CECI
                || TipoEscola == TipoEscola.CEUCEMEI;
        }
    }
}