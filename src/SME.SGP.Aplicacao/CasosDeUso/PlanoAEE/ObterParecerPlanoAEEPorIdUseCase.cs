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
    public class ObterParecerPlanoAEEPorIdUseCase : AbstractUseCase, IObterParecerPlanoAEEPorIdUseCase
    {
        public ObterParecerPlanoAEEPorIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PlanoAEEParecerDto> Executar(long planoAEEId)
        {
            var planoAEE = await mediator.Send(new ObterPlanoAEEPorIdQuery(planoAEEId));

            return await MapearParaDto(planoAEE);
        }

        private async Task<PlanoAEEParecerDto> MapearParaDto(PlanoAEE planoAEE)
        {
            var responsavel = planoAEE.ResponsavelPaaiId.HasValue ?
                await ObterResponsavel(planoAEE.ResponsavelPaaiId.Value) :
                null;

            var usuario = await mediator.Send(ObterUsuarioLogadoQuery.Instance);

            var usuarioCoreSSO = responsavel.NaoEhNulo() ? await mediator.Send(new ObterUsuarioCoreSSOQuery(responsavel.CodigoRf)) : null;

            if (usuarioCoreSSO.NaoEhNulo() && !string.IsNullOrEmpty(usuarioCoreSSO.Nome))
                responsavel.Nome = usuarioCoreSSO.Nome;

            var turma = await mediator.Send(new ObterTurmaComUeEDrePorIdQuery(planoAEE.TurmaId));

            return new PlanoAEEParecerDto()
            {
                ParecerCoordenacao = planoAEE.ParecerCoordenacao,
                ParecerPAAI = planoAEE.ParecerPAAI,
                ResponsavelNome = responsavel?.Nome,
                ResponsavelRF = responsavel?.CodigoRf,
                PodeEditarParecerCoordenacao = await PodeEditarParecerCP(planoAEE, usuario, turma),
                PodeEditarParecerPAAI = PodeEditarParecerPAAI(planoAEE, usuario),
                PodeAtribuirResponsavel = await PodeAtribuirResponsavel(planoAEE, usuario, turma),
                PodeDevolverPlanoAEE = PodeDevolverPlanoAEE(usuario, planoAEE.SituacaoPodeDevolverPlanoAEE()),
            };
        }

        private async Task<bool> PodeAtribuirResponsavel(PlanoAEE planoAEE, Usuario usuario, Turma turma)
            => planoAEE.Situacao.EhUmDosValores(SituacaoPlanoAEE.AtribuicaoPAAI, SituacaoPlanoAEE.ParecerPAAI, SituacaoPlanoAEE.Validado, SituacaoPlanoAEE.Expirado)
               && await EhCoordenadorCEFAI(usuario, turma);

        private async Task<bool> EhCoordenadorCEFAI(Usuario usuarioLogado, Turma turma)
        {
            if (!usuarioLogado.EhCoordenadorCEFAI())
                return false;

            var codigoDre = await mediator.Send(new ObterCodigoDREPorUeIdQuery(turma.UeId));
            if (string.IsNullOrEmpty(codigoDre))
                return false;

            return await UsuarioTemFuncaoCEFAINaDRE(usuarioLogado, codigoDre);
        }

        private async Task<bool> UsuarioTemFuncaoCEFAINaDRE(Usuario usuarioLogado, string codigoDre)
        {
            var funcionarios = await mediator.Send(new ObterFuncionariosDreOuUePorPerfisQuery(codigoDre, new List<Guid>() { Perfis.PERFIL_CEFAI }));
            return funcionarios.Any(c => c.Login == usuarioLogado.CodigoRf);
        }

        private bool PodeEditarParecerPAAI(PlanoAEE planoAEE, Usuario usuario)
            => planoAEE.Situacao.EhUmDosValores(SituacaoPlanoAEE.ParecerPAAI, SituacaoPlanoAEE.Validado, SituacaoPlanoAEE.Expirado)
            && (planoAEE.ResponsavelPaaiId.GetValueOrDefault() == usuario.Id);

        private async Task<bool> PodeEditarParecerCP(PlanoAEE planoAEE, Usuario usuario, Turma turma)
            => SituacaoPermiteEdicaoCP(planoAEE.Situacao) &&
              (usuario.EhGestorEscolar() &&
               await UsuarioGestorDoPlano(usuario, turma) 
              || usuario.EhGestorCIEJA() &&
               await UsuarioGestorDoPlano(usuario, turma));

        private bool SituacaoPermiteEdicaoCP(SituacaoPlanoAEE situacao)
            => new SituacaoPlanoAEE[] 
            { 
                SituacaoPlanoAEE.ParecerCP,
                SituacaoPlanoAEE.AtribuicaoPAAI,
                SituacaoPlanoAEE.ParecerPAAI,
                SituacaoPlanoAEE.Devolvido
            }.Contains(situacao);

        private async Task<bool> UsuarioGestorDoPlano(Usuario usuario, Turma turma)
            => await mediator.Send(new EhGestorDaEscolaQuery(usuario.CodigoRf, turma.Ue.CodigoUe, usuario.PerfilAtual));

        private async Task<Usuario> ObterResponsavel(long responsavelId)
            => await mediator.Send(new ObterUsuarioPorIdQuery(responsavelId));

        private bool PodeDevolverPlanoAEE(Usuario usuario, bool situacaoPodeDevolverPlanoAEE)
        {            
            if (usuario.EhNulo())
                throw new NegocioException("Usuário não localizado");

            if (usuario.EhPerfilProfessor())
                return false;

            if (!situacaoPodeDevolverPlanoAEE)
                return false;

            return true;
        }
    }
}
