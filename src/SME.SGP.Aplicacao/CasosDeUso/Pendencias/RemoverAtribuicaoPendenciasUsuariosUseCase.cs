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
    public class RemoverAtribuicaoPendenciasUsuariosUseCase : AbstractUseCase, IRemoverAtribuicaoPendenciasUsuariosUseCase
    {
        public RemoverAtribuicaoPendenciasUsuariosUseCase(IMediator mediator) : base(mediator)
        { }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            try
            {
                var pendenciaFuncionarios = await mediator.Send(new ObterPendenciaPerfilUsuarioPorSituacaoPendenciaQuery((int)SituacaoPendencia.Pendente));

                var pendenciasComUes = pendenciaFuncionarios.Where(w => w.UeId.HasValue).Select(s => s.UeId.Value).Distinct();
                foreach (var ue in pendenciasComUes)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaRemoverAtribuicaoPendenciaUsuariosUe, MapearFiltroPendenciaPerfilUsuario(pendenciaFuncionarios, ue), Guid.NewGuid(), null));

                var pendenciasSemPendPerfUsuario = await mediator.Send(ObterPendenciaSemPendenciaPerfilUsuarioQuery.Instance);
                foreach (var pendencia in pendenciasSemPendPerfUsuario)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpPendencias.RotaTratarAtribuicaoPendenciaUsuarios, new FiltroTratamentoAtribuicaoPendenciaDto(pendencia.PendenciaId, pendencia.UeId), Guid.NewGuid()));

                var pendenciasSemUes = pendenciaFuncionarios.Where(w => !w.UeId.HasValue).Select(s => s.PendenciaId).Distinct();

                if (pendenciasSemUes?.Count() > 0)
                    throw new NegocioException($"Erro na remoção de atribuição de Pendência Perfil Usuário.");
            }
            catch (Exception)
            {
                throw new NegocioException($"Erro na remoção de atribuição de Pendência Perfil Usuário.");
            }
            return true;
        }

        private FiltroRemoverAtribuicaoPendenciaDto MapearFiltroPendenciaPerfilUsuario(IEnumerable<PendenciaPerfilUsuarioDto> pendenciaFuncionarios, long ue)
        {
            return new FiltroRemoverAtribuicaoPendenciaDto(ue, pendenciaFuncionarios.Where(w => w.UeId == ue));
        }
    }
}
