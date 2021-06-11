using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Utilitarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class NotificarEncerramentoPlanoAEECommandHandler : IRequestHandler<NotificarEncerramentoPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public NotificarEncerramentoPlanoAEECommandHandler(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(NotificarEncerramentoPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var plano = await mediator.Send(new ObterPlanoAEEComTurmaUeEDrePorIdQuery(request.PlanoAEEId));

            var ueDre = $"{plano.Turma.Ue.TipoEscola.ShortName()} {plano.Turma.Ue.Nome} ({plano.Turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = plano.Turma.ModalidadeCodigo == Modalidade.InfantilPreEscola ? "da criança" : "do estudante";

            var titulo = $"Plano AEE encerrado - {plano.AlunoNome} ({plano.AlunoCodigo}) - {ueDre}";
            var descricao = $@"O Plano AEE {estudanteOuCrianca} {plano.AlunoNome} ({plano.AlunoCodigo}) da turma {plano.Turma.NomeComModalidade()} da {ueDre} foi encerrado.<br/>
                            <ul>
                                <li>Devolutiva da Coordenação: {UtilRegex.RemoverTagsHtml(plano.ParecerCoordenacao)}</li>
                                <li>Devolutiva PAAI: {UtilRegex.RemoverTagsHtml(plano.ParecerPAAI)}</li>
                            </ul>
                            <a href='{hostAplicacao}aee/plano/editar/{plano.Id}'>Clique aqui</a> para acessar o plano. ";

            var usuariosIds = await ObterUsuarios(plano.Turma.Ue.CodigoUe, plano.Turma.Ue.Dre.CodigoDre, plano.CriadoRF);

            if (usuariosIds.Any())
                await mediator.Send(new GerarNotificacaoPlanoAEECommand(plano.Id, usuariosIds, titulo, descricao, NotificacaoPlanoAEETipo.PlanoReestruturado, NotificacaoCategoria.Aviso));

            return true;
        }

        private async Task<IEnumerable<long>> ObterUsuarios(string ueCodigo, string dreCodigo, string rfCriador)
        {
            var usuariosRFs = await ObterCoordenadoresUe(ueCodigo);
            usuariosRFs.Add(rfCriador);

            var usuariosIds = await ObterUsuariosId(usuariosRFs);
            return usuariosIds;
        }

        private async Task<List<string>> ObterCoordenadoresUe(string codigoUe)
        {
            var funcionariosCP = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.CP));
            if (funcionariosCP.Any())
                return funcionariosCP.Select(f => f.CodigoRF).ToList();

            var funcionariosAD = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.AD));
            if (funcionariosAD.Any())
                return funcionariosAD.Select(f => f.CodigoRF).ToList();

            var funcionariosDiretor = await mediator.Send(new ObterFuncionariosPorUeECargoQuery(codigoUe, (int)Cargo.Diretor));
            if (funcionariosDiretor.Any())
                return funcionariosDiretor.Select(f => f.CodigoRF).ToList();

            return null;
        }

        private async Task<List<long>> ObterUsuariosId(List<string> funcionarios)
        {
            List<long> usuarios = new List<long>();
            foreach (var functionario in funcionarios)
            {
                var usuario = await mediator.Send(new ObterUsuarioIdPorRfOuCriaQuery(functionario));
                usuarios.Add(usuario);
            }
            return usuarios;
        }
    }
}
