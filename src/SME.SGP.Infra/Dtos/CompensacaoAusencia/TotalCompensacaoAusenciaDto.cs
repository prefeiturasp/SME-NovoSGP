using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra.Dtos
{
    public class TotalCompensacaoAusenciaDto
    {
        public long TotalAulas { get; set; }
        public long TotalCompensacoes { get; set; }

        public string TotalCompensacoesFormatado
        {
            get => $"{TotalCompensacoes} ausências compensadas ({PercentualCompensacoes}% das aulas)";
        }
        public double PercentualCompensacoes
        {
            get
            {
                if (TotalAulas == 0)
                    return 0;

                var porcentagem = ((double)TotalCompensacoes / TotalAulas ) * 100;

                return Math.Round(porcentagem > 100 ? 100 : porcentagem, 2);
            }
        }
    }
}
