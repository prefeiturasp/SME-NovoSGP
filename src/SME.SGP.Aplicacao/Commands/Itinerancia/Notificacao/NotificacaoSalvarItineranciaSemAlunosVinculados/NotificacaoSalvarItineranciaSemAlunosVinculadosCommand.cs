using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class NotificacaoSalvarItineranciaSemAlunosVinculadosCommand : IRequest<bool>
    {
        public NotificacaoSalvarItineranciaSemAlunosVinculadosCommand(string ueCodigo, string criadoRF, string criadoPor, DateTime dataVisita)
        {
            UeCodigo = ueCodigo;
            CriadoRF = criadoRF;
            CriadoPor = criadoPor;
            DataVisita = dataVisita;
        }

        public string UeCodigo { get; set; }
        public string CriadoRF { get; set; }
        public string CriadoPor { get; set; }
        public DateTime DataVisita { get; set; }
    }
}
