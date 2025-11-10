using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarUsuarioAlteracaoExtemporaneaUseCase : AbstractUseCase, INotificarUsuarioAlteracaoExtemporaneaUseCase
    {
        private readonly IServicoUsuario servicoUsuario;

        public NotificarUsuarioAlteracaoExtemporaneaUseCase(IMediator mediator, IServicoUsuario servicoUsuario) : base(mediator)
        {
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroNotificarUsuarioAlteracaoExtemporaneaDto>();

            var usuariosCPs = await ObterUsuariosPorCodigoUeETipo(filtro.CodigoUe, Cargo.CP);

            foreach (var usuarioCP in usuariosCPs)
                if (usuarioCP != null)
                    await NotificarUsuario(filtro, usuarioCP);

            var usuarioDiretor = (await ObterUsuariosPorCodigoUeETipo(filtro.CodigoUe, Cargo.CP)).FirstOrDefault();

            if (usuarioDiretor != null)
                await NotificarUsuario(filtro, usuarioDiretor);

            return true;
        }

        private Task NotificarUsuario(FiltroNotificarUsuarioAlteracaoExtemporaneaDto filtro, Usuario usuario)
        {
            return mediator.Send(new NotificarUsuarioCommand(
                $"Alteração em Atividade Avaliativa - Turma {filtro.TurmaNome}",
                filtro.MensagemNotificacao,
                usuario.CodigoRf,
                NotificacaoCategoria.Alerta,
                NotificacaoTipo.Notas,
                filtro.AtividadeAvaliativa.DreId,
                filtro.AtividadeAvaliativa.UeId,
                filtro.AtividadeAvaliativa.TurmaId,
                usuarioId: usuario.Id
                ));
        }

        private async Task<IEnumerable<Usuario>> ObterUsuariosPorCodigoUeETipo(string codigoUe, Cargo tipo)
        {
            var usuarios = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)tipo));

            return await CarregaUsuariosPorRFs(usuarios);
        }

        private async Task<IEnumerable<Usuario>> CarregaUsuariosPorRFs(IEnumerable<FuncionarioDTO> listaCPsUe)
        {
            var usuarios = new List<Usuario>();
            foreach (var cpUe in listaCPsUe)
            {
                var usuario = await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(cpUe.CodigoRF);
                if (usuario != null)
                    usuarios.Add(usuario);
            }
            return usuarios;
        }
    }
}
