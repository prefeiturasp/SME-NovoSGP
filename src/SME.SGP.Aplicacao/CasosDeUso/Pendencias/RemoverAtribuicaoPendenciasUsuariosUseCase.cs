using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverAtribuicaoPendenciasUsuariosUseCase : AbstractUseCase, IRemoverAtribuicaoPendenciasUsuariosUseCase
    {
        public RemoverAtribuicaoPendenciasUsuariosUseCase(IMediator mediator) : base(mediator)
        {}

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var pendenciaFuncionarios = await mediator.Send(new ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQuery((int)SituacaoPendencia.Pendente));

                var pendenciasComUes = pendenciaFuncionarios.Where(w => w.UeId.HasValue).Select(s => s.UeId.Value).Distinct();
                foreach (var ue in pendenciasComUes)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaRemoverAtribuicaoPendenciaUsuariosUe, MapearFiltroPendenciaPerfilUsuario(pendenciaFuncionarios, ue), Guid.NewGuid(), null));

                var pendenciasSemUes = pendenciaFuncionarios.Where(w => !w.UeId.HasValue).Select(s => s.PendenciaId).Distinct();
                foreach (var pendencia in pendenciasSemUes)
                    await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na remoção de atribuição de Pendência Perfil Usuário.", LogNivel.Negocio, LogContexto.Pendencia, $"Pendência sem UE: {pendencia}"));

            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro na remoção de atribuição de Pendência Perfil Usuário.", LogNivel.Negocio, LogContexto.Pendencia, ex.Message));
            }
            return true;
        }

        private FiltroRemoverAtribuicaoPendenciaDto MapearFiltroPendenciaPerfilUsuario(IEnumerable<PendenciaPerfilUsuarioDto> pendenciaFuncionarios, long ue)
        {
            return new FiltroRemoverAtribuicaoPendenciaDto(ue, pendenciaFuncionarios.Where(w => w.UeId == ue));
        }
    }
}
