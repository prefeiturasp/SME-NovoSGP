using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAulaPrevista : IConsultasAulaPrevista
    {
        private readonly IRepositorioAulaPrevista repositorio;

        public ConsultasAulaPrevista(IRepositorioAulaPrevista repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<IEnumerable<AulasPrevistasDadasDto>> ObterAulaPrevistaDada(Modalidade modalidade, string turmaId, string disciplinaId)
        {
            int tipoCalendarioId = (int)ModalidadeParaModalidadeTipoCalendario(modalidade);
            return await repositorio.ObterAulaPrevistaDada(tipoCalendarioId, turmaId, disciplinaId);
        }

        private ModalidadeTipoCalendario ModalidadeParaModalidadeTipoCalendario(Modalidade modalidade)
        {
            switch (modalidade)
            {
                case Modalidade.EJA:
                    return ModalidadeTipoCalendario.EJA;

                default:
                    return ModalidadeTipoCalendario.FundamentalMedio;
            }
        }
    }
}
