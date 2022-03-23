using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class GerarPendenciaAusenciaRegistroIndividualUseCase : AbstractUseCase, IGerarPendenciaAusenciaRegistroIndividualUseCase
    {
        public GerarPendenciaAusenciaRegistroIndividualUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var parametroCorrespondente = await mediator.Send(new ObterParametroSistemaPorTipoQuery(TipoParametroSistema.PendenciaPorAusenciaDeRegistroIndividual));
                if (string.IsNullOrWhiteSpace(parametroCorrespondente))
                    return true;

                var turmasDoEnsinoInfantil = await mediator.Send(new ObterTurmasPorAnoModalidadeQuery(DateTime.Now.Year, Modalidade.EducacaoInfantil));
                if (!turmasDoEnsinoInfantil?.Any() ?? true)
                {
                    await mediator.Send(new SalvarLogViaRabbitCommand("Não foram encontradas turmas para geração de pendências de ausência de registro individual.", LogNivel.Negocio, LogContexto.RegistroIndividual));
                    return false;
                }

                foreach (var turma in turmasDoEnsinoInfantil)
                {
                    await GerarPendenciaAusenciaRegistroIndividualTurmaAsync(turma);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task GerarPendenciaAusenciaRegistroIndividualTurmaAsync(Turma turma)
        {
            if (turma is null) return;

            var retorno = await mediator.Send(new GerarPendenciaAusenciaRegistroIndividualTurmaCommand(turma));
            if (retorno is null)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível gerar a pendência de ausência de registro individual para a turma {turma.Id}.", LogNivel.Negocio, LogContexto.RegistroIndividual));
                return;
            }

            if (retorno.ExistemErros)
            {
                var erros = string.Join(", ", retorno.Mensagens);
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível gerar a pendência de ausência de registro individual para a turma {turma.Id}. {erros}", LogNivel.Negocio, LogContexto.RegistroIndividual));
                return;
            }
        }
    }
}