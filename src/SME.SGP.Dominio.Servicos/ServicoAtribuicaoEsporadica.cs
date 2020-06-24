using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Servicos
{
    public class ServicoAtribuicaoEsporadica : IServicoAtribuicaoEsporadica
    {
        private readonly IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica;
        private readonly IRepositorioPeriodoEscolar repositorioPeriodoEscolar;
        private readonly IRepositorioTipoCalendario repositorioTipoCalendario;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;
        private readonly IUnitOfWork unitOfWork;

        public ServicoAtribuicaoEsporadica(IRepositorioPeriodoEscolar repositorioPeriodoEscolar, IRepositorioTipoCalendario repositorioTipoCalendario,
            IRepositorioAtribuicaoEsporadica repositorioAtribuicaoEsporadica, IServicoUsuario servicoUsuario, IServicoEol servicoEOL, IUnitOfWork unitOfWork)
        {
            this.repositorioPeriodoEscolar = repositorioPeriodoEscolar ?? throw new System.ArgumentNullException(nameof(repositorioPeriodoEscolar));
            this.repositorioTipoCalendario = repositorioTipoCalendario ?? throw new System.ArgumentNullException(nameof(repositorioTipoCalendario));
            this.repositorioAtribuicaoEsporadica = repositorioAtribuicaoEsporadica ?? throw new System.ArgumentNullException(nameof(repositorioAtribuicaoEsporadica));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
            this.servicoEOL = servicoEOL ?? throw new System.ArgumentNullException(nameof(servicoEOL));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task Salvar(AtribuicaoEsporadica atribuicaoEsporadica, int anoLetivo)
        {
            var atribuicoesConflitantes = repositorioAtribuicaoEsporadica.ObterAtribuicoesDatasConflitantes(atribuicaoEsporadica.DataInicio, atribuicaoEsporadica.DataFim, atribuicaoEsporadica.ProfessorRf, atribuicaoEsporadica.Id);

            if (atribuicoesConflitantes != null && atribuicoesConflitantes.Any())
                throw new NegocioException("Já existem outras atribuições, para este professor, no periodo especificado");

            var tipoCalendario = await repositorioTipoCalendario.BuscarPorAnoLetivoEModalidade(anoLetivo, ModalidadeTipoCalendario.FundamentalMedio);

            if (tipoCalendario == null)
                throw new NegocioException("Nenhum tipo de calendario para o ano letivo vigente encontrado");

            var periodosEscolares = await repositorioPeriodoEscolar.ObterPorTipoCalendario(tipoCalendario.Id);

            if (periodosEscolares == null || !periodosEscolares.Any())
                throw new NegocioException("Nenhum periodo escolar encontrado para o ano letivo vigente");

            bool ehPerfilSelecionadoSME = servicoUsuario.UsuarioLogadoPossuiPerfilSme();

            atribuicaoEsporadica.Validar(ehPerfilSelecionadoSME, anoLetivo, periodosEscolares);

            using (var transacao = unitOfWork.IniciarTransacao())
            {
                repositorioAtribuicaoEsporadica.Salvar(atribuicaoEsporadica);

                AdicionarAtribuicaoEOL(atribuicaoEsporadica.ProfessorRf).Wait();

                unitOfWork.PersistirTransacao();
            }
        }

        private async Task AdicionarAtribuicaoEOL(string codigoRF)
        {
            try
            {
                await servicoEOL.AtribuirCJSeNecessario(codigoRF);
            }
            catch (Exception)
            {
                throw new NegocioException("Não foi possivel realizar a atribuição esporadica, por favor contate o suporte");
            }
        }
    }
}