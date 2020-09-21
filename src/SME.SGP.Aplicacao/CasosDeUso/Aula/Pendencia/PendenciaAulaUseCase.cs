using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaUseCase : IPendenciaAulaUseCase
    {
        private readonly IRepositorioPendenciaAula repositorioPendenciaAula;

        public PendenciaAulaUseCase(IRepositorioPendenciaAula repositorioPendenciaAula)
        {
            this.repositorioPendenciaAula = repositorioPendenciaAula ?? throw new ArgumentNullException(nameof(repositorioPendenciaAula));
        }

        #region Metodos Publicos
        public async Task Executar()
        {

            await VerificaPendenciasDiarioDeBordo();
            await VerificaPendenciasAvaliacao();
            await VerificaPendenciasFrequencia();
            await VerificaPendenciasPlanoAula();

        }
        #endregion

        #region Metodos Privados
        private async Task VerificaPendenciasDiarioDeBordo()
        {
            var aulas = await repositorioPendenciaAula.ListarPendenciasPorTipo(TipoPendenciaAula.DiarioBordo, "diario_bordo",
                new long[] { (int)Modalidade.Infantil });
            if (aulas != null)
            {

                await RegistraPendencia(aulas, TipoPendenciaAula.DiarioBordo);

            }
        }

        private async Task VerificaPendenciasAvaliacao()
        {
            var aulas = await repositorioPendenciaAula.ListarPendenciasAtividadeAvaliativa();
            if (aulas != null)
            {
                await RegistraPendencia(aulas, TipoPendenciaAula.Avaliacao);
            }

        }

        private async Task VerificaPendenciasFrequencia()
        {
            var aulas = await repositorioPendenciaAula.ListarPendenciasPorTipo(TipoPendenciaAula.Frequencia, "registro_frequencia",
                new long[] { (int)Modalidade.Infantil, (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio });
            if (aulas != null)
            {
                await RegistraPendencia(aulas, TipoPendenciaAula.Frequencia);

            }
        }

        private async Task VerificaPendenciasPlanoAula()
        {
            var aulas = await repositorioPendenciaAula.ListarPendenciasPorTipo(TipoPendenciaAula.PlanoAula, "plano_aula",
                new long[] { (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio });
            if (aulas != null)
            {
                await RegistraPendencia(aulas, TipoPendenciaAula.PlanoAula);

            }
        }

        private async Task RegistraPendencia(IEnumerable<Aula> aulas, TipoPendenciaAula tipoPendenciaAula)
        {
            repositorioPendenciaAula.SalvarVarias(aulas, tipoPendenciaAula);
        }

        #endregion
    }
}
