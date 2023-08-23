using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SME.SGP.Dominio.Constantes.MensagensNegocio;
using Newtonsoft.Json;

namespace SME.SGP.Aplicacao
{
    /// <summary>
    /// Método de encerramento de planos para alunos que estejam inativos e com situação concluída.
    /// </summary>
    public class EncerrarPlanosAEEEstudantesInativosUseCase : AbstractUseCase, IEncerrarPlanosAEEEstudantesInativosUseCase
    {
        public EncerrarPlanosAEEEstudantesInativosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var planosAtivos = await mediator.Send(ObterPlanosAEEAtivosQuery.Instance);
            var usuarioSistema = await mediator.Send(new ObterUsuarioPorRfQuery("Sistema"));

            if (planosAtivos != null && planosAtivos.Any())
            {
                foreach(var plano in planosAtivos)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAEE.EncerrarPlanoAEEEstudantesInativosTratar, plano, Guid.NewGuid(), usuarioSistema));
               
                return true;
            }

            return false;
        }
    }
}