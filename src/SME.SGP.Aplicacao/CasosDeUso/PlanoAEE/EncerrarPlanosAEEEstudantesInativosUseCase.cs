using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EncerrarPlanosAEEEstudantesInativosUseCase : AbstractUseCase, IEncerrarPlanosAEEEstudantesInativosUseCase
    {
        public EncerrarPlanosAEEEstudantesInativosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var planosAtivos = await mediator.Send(new ObterPlanosAEEAtivosQuery());

            foreach(var planoAEE in planosAtivos)
            {
                var aluno = await mediator.Send(new ObterAlunoPorCodigoEolQuery(planoAEE.AlunoCodigo, planoAEE.CriadoEm.Year));

                if (aluno.EstaInativo(DateTime.Today))
                    await EncerrarPlanoAEE(planoAEE);
            }

            return true;
        }

        private async Task EncerrarPlanoAEE(PlanoAEE planoAEE)
        {
            planoAEE.Situacao = SituacaoPlanoAEE.EncerradoAutomaticamento;

            await mediator.Send(new PersistirPlanoAEECommand(planoAEE));
        }
    }
}
