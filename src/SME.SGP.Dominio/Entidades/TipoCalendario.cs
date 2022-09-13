﻿namespace SME.SGP.Dominio
{
    public class TipoCalendario : EntidadeBase
    {
        public int AnoLetivo { get; set; }
        public bool Excluido { get; set; }
        public bool Migrado { get; set; }
        public ModalidadeTipoCalendario Modalidade { get; set; }
        public string Nome { get; set; }
        public Periodo Periodo { get; set; }
        public bool Situacao { get; set; }

        public int QuantidadeDeBimestres()
        {
            if (Modalidade == ModalidadeTipoCalendario.EJA && AnoLetivo > 2021)
                return 2;
 
            return 4;
        }

        public Modalidade ObterModalidadeTurma()
        {
            return Modalidade == ModalidadeTipoCalendario.EJA ?
                    Dominio.Modalidade.EJA :
                    Modalidade == ModalidadeTipoCalendario.Infantil ?
                    Dominio.Modalidade.EducacaoInfantil :
                    Dominio.Modalidade.Medio;
        }
    }
}