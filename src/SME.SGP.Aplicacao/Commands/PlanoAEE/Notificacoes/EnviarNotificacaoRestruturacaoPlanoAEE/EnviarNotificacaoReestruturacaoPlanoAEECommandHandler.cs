using MediatR;
using Microsoft.Extensions.Configuration;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarNotificacaoReestruturacaoPlanoAEECommandHandler : IRequestHandler<EnviarNotificacaoReestruturacaoPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IConfiguration configuration;

        public EnviarNotificacaoReestruturacaoPlanoAEECommandHandler(IMediator mediator, IConfiguration configuration)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<bool> Handle(EnviarNotificacaoReestruturacaoPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var reestruturacao = await mediator.Send(new ObterPlanoAEEReestruturacaoPorIdQuery(request.ReestruturacaoId));

            if (reestruturacao == null)
                throw new NegocioException($"A reestruturação [{request.ReestruturacaoId}] não foi localizada.");

            var plano = reestruturacao.PlanoAEEVersao.PlanoAEE;

            var ueDre = $"{plano.Turma.Ue.TipoEscola.ShortName()} {plano.Turma.Ue.Nome} ({plano.Turma.Ue.Dre.Abreviacao})";
            var hostAplicacao = configuration["UrlFrontEnd"];
            var estudanteOuCrianca = plano.Turma.ModalidadeCodigo == Modalidade.EducacaoInfantil ? "da criança" : "do estudante";

            var titulo = $"Reestruturação do plano AEE - {plano.AlunoNome} ({plano.AlunoCodigo}) - {ueDre}";
            var descricao = $@"O usuário {request.Usuario.Nome} ({request.Usuario.CodigoRf}) registrou a reestruturação {reestruturacao.Semestre}º semestre do Plano AEE 
                            {estudanteOuCrianca} {plano.AlunoNome} ({plano.AlunoCodigo}) da turma {plano.Turma.NomeComModalidade()} da {ueDre} em {reestruturacao.CriadoEm:dd/MM/yyyy}.<br/>
                            <a href='{hostAplicacao}aee/plano/editar/{plano.Id}'>Clique aqui</a> para acessar o plano. ";

            var usuariosIds = await ObterUsuarios(plano.Turma.Ue.CodigoUe, plano.Turma.Ue.Dre.CodigoDre);

            if (usuariosIds.Any())
                await mediator.Send(new GerarNotificacaoPlanoAEECommand(plano.Id, usuariosIds, titulo, descricao, NotificacaoPlanoAEETipo.PlanoReestruturado, NotificacaoCategoria.Aviso));

            return true;
        }

        private async Task<IEnumerable<long>> ObterUsuarios(string ueCodigo, string dreCodigo)
        {
            var coordenadoresUe = await ObterCoordenadoresUe(ueCodigo);

            var usuariosIds = await ObterUsuariosId(coordenadoresUe);
            var coordenadorCEFAI = await mediator.Send(new ObtemUsuarioCEFAIDaDreQuery(dreCodigo));

            if (coordenadorCEFAI != 0)
                usuariosIds.Add(coordenadorCEFAI);

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
