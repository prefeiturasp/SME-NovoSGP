using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Dominio.Entidades
{
    public class PeriodoEscolar : EntidadeBase
    {
        public long TipoCalendario { get; set; }
        public int Bimestre { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }

        public void Validar(int anoBase, bool eja)
        {
            ValidarCamposNaoInformados();
            ValidarBimestre(eja);
            ValidarAnoBase(anoBase);
            ValidarIncioBimestre();
        }

        private void ValidarIncioBimestre()
        {
            if (PeriodoFim < PeriodoInicio)
                throw new NegocioException($"{Bimestre}º Bimestre: A data de inicio não pode ser posterior a data de fim do periodo");
        }

        private void ValidarBimestre(bool eja)
        {
            int maxBimestre = eja ? 2 : 4;

            if (Bimestre < 1 || Bimestre > maxBimestre)
                throw new NegocioException($"O bimestre do periodo não pode ser menor que 1 ou maior que {maxBimestre}");

        }

        private void ValidarCamposNaoInformados()
        {
            if (TipoCalendario == 0)
                throw new NegocioException("Deve ser informado o tipo de calendario");

            if (Bimestre == 0)
                throw new NegocioException("Deve ser informado o bimestre");

            if (PeriodoInicio == null)
                throw new NegocioException("Deve ser informado o incio do periodo");

            if (PeriodoFim == null)
                throw new NegocioException("Deve ser informado o fim do periodo");
        }

        private void ValidarAnoBase(int anoBase)
        {
            if (PeriodoInicio.Year < anoBase)
                throw new NegocioException("O incio do periodo não pode ser anterior ao ano base");

            if (PeriodoFim.Year > anoBase)
                throw new NegocioException("O fim do periodo não pode ser posterior ao ano base");

            if (PeriodoInicio.Year > anoBase)
                throw new NegocioException("O incio do periodo não pode ser posterior ao ano base");

            if (PeriodoFim.Year < anoBase)
                throw new NegocioException("O fim do periodo não pode ser anterior ao ano base");
        }
    }
}
