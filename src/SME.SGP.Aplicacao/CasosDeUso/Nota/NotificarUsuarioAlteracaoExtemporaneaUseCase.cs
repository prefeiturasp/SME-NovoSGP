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
        private readonly IRepositorioConceitoConsulta repositorioConceito;
        private readonly IConsultasAbrangencia consultasAbrangencia;
        private readonly IRepositorioNotaParametro repositorioNotaParametro;
        private readonly IRepositorioAulaConsulta repositorioAula;
        private readonly IRepositorioTurmaConsulta repositorioTurma;
        private readonly IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioCiclo repositorioCiclo;
        private readonly IServicoUsuario servicoUsuario;

        public NotificarUsuarioAlteracaoExtemporaneaUseCase(IMediator mediator, IRepositorioConceitoConsulta repositorioConceito, 
                                                            IConsultasAbrangencia consultasAbrangencia, IRepositorioNotaParametro repositorioNotaParametro,
                                                            IRepositorioAulaConsulta repositorioAula, IRepositorioTurmaConsulta repositorioTurma,
                                                            IRepositorioNotaTipoValorConsulta repositorioNotaTipoValor, IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar,
                                                            IRepositorioCiclo repositorioCiclo, IServicoUsuario servicoUsuario) : base(mediator)
        {
            this.repositorioConceito = repositorioConceito ?? throw new ArgumentNullException(nameof(repositorioConceito));
            this.consultasAbrangencia = consultasAbrangencia ?? throw new ArgumentNullException(nameof(consultasAbrangencia));
            this.repositorioNotaParametro = repositorioNotaParametro ?? throw new ArgumentNullException(nameof(repositorioNotaParametro));
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
            this.repositorioTurma = repositorioTurma ?? throw new ArgumentNullException(nameof(repositorioTurma));
            this.repositorioNotaTipoValor = repositorioNotaTipoValor ?? throw new ArgumentNullException(nameof(repositorioNotaTipoValor));
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioCiclo = repositorioCiclo ?? throw new ArgumentNullException(nameof(repositorioCiclo));
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroNotificarUsuarioAlteracaoExtemporaneaDto>();

            var usuariosCPs = await ObterUsuariosPorCodigoUeETipo(filtro.CodigoUe, Cargo.CP);

            foreach (var usuarioCP in usuariosCPs)
                await NotificarUsuario(filtro, usuarioCP);

            var usuarioDiretor = (await ObterUsuariosPorCodigoUeETipo(filtro.CodigoUe, Cargo.CP)).FirstOrDefault();

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
                usuarios.Add(await servicoUsuario.ObterUsuarioPorCodigoRfLoginOuAdiciona(cpUe.CodigoRF));
            return usuarios;
        }
    }
}
