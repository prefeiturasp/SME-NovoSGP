using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoAtribuicaoEsporadica : IServicoAtribuicaoEsporadica
    {
        private readonly IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica;
        private readonly IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendarioConsulta repositorioTipoCalendario;
        private readonly IMediator mediator;
        private readonly IServicoUsuario servicoUsuario;

        public ServicoAtribuicaoEsporadica(IRepositorioPeriodoEscolarConsulta repositorioPeriodoEscolar, IRepositorioTipoCalendarioConsulta repositorioTipoCalendario,
            IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica, IServicoUsuario servicoUsuario, IMediator mediator)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Salvar(AtribuicaoEsporadica atribuicaoEsporadica, int anoLetivo, bool ehInfantil)
        {
            var atribuicoesConflitantes = repositorioAtribuicaoEsporadica.ObterAtribuicoesDatasConflitantes(atribuicaoEsporadica.DataInicio, atribuicaoEsporadica.DataFim, atribuicaoEsporadica.ProfessorRf, atribuicaoEsporadica.DreId, atribuicaoEsporadica.UeId, atribuicaoEsporadica.Id);

            if (atribuicoesConflitantes.NaoEhNulo() && atribuicoesConflitantes.Any())
                throw new NegocioException("Já existem outras atribuições, para este professor, no periodo especificado");

            var modalidade = ehInfantil ? ModalidadeTipoCalendario.Infantil : ModalidadeTipoCalendario.FundamentalMedio; //E se for outra modalidade?

            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, modalidade);

            if (tipoCalendario.EhNulo())
                throw new NegocioException("Nenhum tipo de calendario para o ano letivo vigente encontrado");

            var periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);

            if (periodosEscolares.EhNulo() || !periodosEscolares.Any())
                throw new NegocioException("Nenhum periodo escolar encontrado para o ano letivo vigente");

            bool ehPerfilSelecionadoSME = servicoUsuario.UsuarioLogadoPossuiPerfilSme();

            atribuicaoEsporadica.Validar(ehPerfilSelecionadoSME, anoLetivo, periodosEscolares, modalidade);

            repositorioAtribuicaoEsporadica.Salvar(atribuicaoEsporadica);

            Guid perfilAtribuicao = ehInfantil ? Perfis.PERFIL_CJ_INFANTIL : Perfis.PERFIL_CJ;

            await AdicionarAtribuicaoEOL(atribuicaoEsporadica.ProfessorRf, perfilAtribuicao);
        }

        private async Task AdicionarAtribuicaoEOL(string codigoRF, Guid perfil)
        {
            await mediator.Send(new AtribuirPerfilCommand(codigoRF, perfil));
        }
    }
}