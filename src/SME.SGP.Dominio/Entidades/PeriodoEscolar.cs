using System;
using System.Collections.Generic;

namespace SME.SGP.Dominio
{
    public class PeriodoEscolar : EntidadeBase
    {
        public int Bimestre { get; set; }
        public bool Migrado { get; set; }
        public DateTime PeriodoFim { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public TipoCalendario TipoCalendario { get; set; }
        public long TipoCalendarioId { get; set; }

        public void AdicionarTipoCalendario(TipoCalendario tipoCalendario)
        {
            TipoCalendario = tipoCalendario;
            TipoCalendarioId = tipoCalendario.Id;
        }

        public bool DataDentroPeriodo(DateTime data)
        {
            return PeriodoInicio.Date <= data.Date && PeriodoFim.Date >= data.Date;
        }

        public IEnumerable<DateTime> ObterIntervaloDatas()
        {
            var datas = new List<DateTime>();
            for (var dia = PeriodoInicio.Date; dia <= PeriodoFim.Date; dia = dia.AddDays(1))
                datas.Add(dia);
            return datas;
        }

        public void Validar(int anoBase, bool eja)
        {
            ValidarCamposObrigatorios();
            NumeroBimestreValido(eja);
            PeriodoDeveEstarDentroAnoBase(anoBase);
            DataInicioPeriodoEMaiorDataFimPeriodo();
        }

        private void DataInicioPeriodoEMaiorDataFimPeriodo()
        {
            if (PeriodoFim < PeriodoInicio)
                throw new NegocioException($"{Bimestre}º Bimestre: A data de início não pode ser posterior a data de fim do período");
        }

        private void NumeroBimestreValido(bool eja)
        {
            int maxBimestre = eja ? 2 : 4;

            if (Bimestre < 1 || Bimestre > maxBimestre)
                throw new NegocioException($"O bimestre do período não pode ser menor que 1 ou maior que {maxBimestre}");
        }

        private void PeriodoDeveEstarDentroAnoBase(int anoBase)
        {
            if (PeriodoInicio.Year < anoBase)
                throw new NegocioException("O início do período não pode ser anterior ao ano base");

            if (PeriodoInicio.Year > anoBase)
                throw new NegocioException("O início do período não pode ser posterior ao ano base");

            if (PeriodoFim.Year < anoBase)
                throw new NegocioException("O fim do período não pode ser anterior ao ano base");

            if (PeriodoFim.Year > anoBase)
                throw new NegocioException("O fim do período não pode ser posterior ao ano base");
        }

        private void ValidarCamposObrigatorios()
        {
            if (TipoCalendarioId == 0)
                throw new NegocioException("Deve ser informado o tipo de calendário");

            if (Bimestre == 0)
                throw new NegocioException("Deve ser informado o bimestre");

            if (PeriodoInicio == DateTime.MinValue)
                throw new NegocioException("Deve ser informado o início do período");

            if (PeriodoFim == DateTime.MinValue)
                throw new NegocioException("Deve ser informado o fim do período");
        }

        public IEnumerable<DateTime> DatasDoPeriodo()
        {
            for (var data = PeriodoInicio; data <= PeriodoFim; data = data.AddDays(1))
                yield return data;
        }

        public IEnumerable<int> MesesDoPeriodo()
        {
            for (int mes = PeriodoInicio.Month; mes <= PeriodoFim.Month; mes++)
                yield return mes;
        }
    }
}