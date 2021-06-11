using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaUseCase : IPendenciaAulaUseCase
    {
        private readonly IMediator mediator;

        public PendenciaAulaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        #region Metodos Publicos

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await VerificaPendenciasDiarioDeBordo();
            await VerificaPendenciasAvaliacao();
            await VerificaPendenciasFrequencia();
            await VerificaPendenciasPlanoAula();

            return true;
        }
        #endregion

        #region Metodos Privados
        private async Task VerificaPendenciasDiarioDeBordo()
        {
            var aulas = await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.DiarioBordo, "diario_bordo",
                new long[] { (int)Modalidade.InfantilPreEscola }));
            if (aulas != null)
            {
                await RegistraPendencia(aulas, TipoPendencia.DiarioBordo);
            }
        }

        private async Task VerificaPendenciasAvaliacao()
        {
            var aulas = await mediator.Send(new ObterPendenciasAtividadeAvaliativaQuery());
            if (aulas != null)
                await RegistraPendencia(aulas, TipoPendencia.Avaliacao);
        }

        private async Task VerificaPendenciasFrequencia()
        {
            var aulas = await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.Frequencia, "registro_frequencia",
                new long[] { (int)Modalidade.InfantilPreEscola, (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio }));
            if (aulas != null)
            {
                await RegistraPendencia(aulas, TipoPendencia.Frequencia);

            }
        }

        private async Task VerificaPendenciasPlanoAula()
        {
            var aulas = await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.PlanoAula, "plano_aula",
                new long[] { (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio }));
            if (aulas != null)
            {
                await RegistraPendencia(aulas, TipoPendencia.PlanoAula);

            }
        }

        private async Task RegistraPendencia(IEnumerable<Aula> aulas, TipoPendencia tipoPendenciaAula)
        {
            var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(tipoPendenciaAula));

            await mediator.Send(new SalvarPendenciasAulasCommand(pendenciaId, aulas.Select(a => a.Id)));
            await SalvarPendenciaUsuario(pendenciaId, aulas.First().ProfessorRf);
        }

        private async Task SalvarPendenciaUsuario(long pendenciaId, string professorRf)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(professorRf));
            await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));
        }
        #endregion
    }
}
