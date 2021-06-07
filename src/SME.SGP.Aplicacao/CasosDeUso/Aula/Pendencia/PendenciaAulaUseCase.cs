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
        private readonly IUnitOfWork unitOfWork;

        public PendenciaAulaUseCase(IMediator mediator,
                                    IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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
            var aulas = mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.DiarioBordo, "diario_bordo",
                new long[] { (int)Modalidade.Infantil })).Result;

            if (aulas != null && aulas.Any())
                await RegistraPendencia(aulas, TipoPendencia.DiarioBordo);
        }

        private async Task VerificaPendenciasAvaliacao()
        {
            var aulas = await mediator.Send(new ObterPendenciasAtividadeAvaliativaQuery());
            if (aulas != null && aulas.Any())
                await RegistraPendencia(aulas, TipoPendencia.Avaliacao);
        }

        private async Task VerificaPendenciasFrequencia()
        {
            var aulas = await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.Frequencia, "registro_frequencia",
                new long[] { (int)Modalidade.Infantil, (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio }));

            try
            {
                var aulasRegistramFrequencia = aulas.Where(a => a.PermiteRegistroFrequencia());
                if (aulasRegistramFrequencia.Any())
                    await RegistraPendencia(aulasRegistramFrequencia, TipoPendencia.Frequencia);
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }
        }

        private async Task VerificaPendenciasPlanoAula()
        {
            var aulas = await mediator.Send(new ObterPendenciasAulasPorTipoQuery(TipoPendencia.PlanoAula, "plano_aula",
                new long[] { (int)Modalidade.Fundamental, (int)Modalidade.EJA, (int)Modalidade.Medio }));

            if (aulas != null && aulas.Any())
                await RegistraPendencia(aulas, TipoPendencia.PlanoAula);
        }

        private async Task RegistraPendencia(IEnumerable<Aula> aulas, TipoPendencia tipoPendenciaAula)
        {
            unitOfWork.IniciarTransacao();

            var pendenciaId = await mediator.Send(new SalvarPendenciaCommand(tipoPendenciaAula));

            await mediator.Send(new SalvarPendenciasAulasCommand(pendenciaId, aulas.Select(a => a.Id)));
            await SalvarPendenciaUsuario(pendenciaId, aulas.First().ProfessorRf);

            unitOfWork.PersistirTransacao();
        }

        private async Task SalvarPendenciaUsuario(long pendenciaId, string professorRf)
        {
            var usuarioId = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(professorRf));
            await mediator.Send(new SalvarPendenciaUsuarioCommand(pendenciaId, usuarioId));
        }
        #endregion
    }
}
