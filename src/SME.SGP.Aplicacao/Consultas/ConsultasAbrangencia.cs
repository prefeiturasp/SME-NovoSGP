using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Dto;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsultasAbrangencia : IConsultasAbrangencia
    {
        private readonly IRepositorioAbrangencia repositorioAbrangencia;
        private readonly IServicoUsuario servicoUsuario;

        public ConsultasAbrangencia(IRepositorioAbrangencia repositorioAbrangencia, IServicoUsuario servicoUsuario)
        {
            this.repositorioAbrangencia = repositorioAbrangencia ?? throw new System.ArgumentNullException(nameof(repositorioAbrangencia));
            this.servicoUsuario = servicoUsuario ?? throw new System.ArgumentNullException(nameof(servicoUsuario));
        }

        public async Task<IEnumerable<AbrangenciaFiltroRetorno>> ObterAbrangenciaPorfiltro(string texto)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            return await repositorioAbrangencia.ObterAbrangenciaPorFiltro(texto, login, perfil);
        }

        public async Task<IEnumerable<AbrangenciaDreRetorno>> ObterDres(Modalidade? modalidade)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            return await repositorioAbrangencia.ObterDres(login, perfil, modalidade);
        }

        public async Task<IEnumerable<EnumeradoRetornoDto>> ObterModalidades()
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var lista = await repositorioAbrangencia.ObterModalidades(login, perfil);

            return from a in lista
                   select new EnumeradoRetornoDto() { Id = a, Descricao = ((Modalidade)a).GetAttribute<DisplayAttribute>().Name };
        }

        public async Task<IEnumerable<int>> ObterSemestres(Modalidade modalidade)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            var retorno = await repositorioAbrangencia.ObterSemestres(login, perfil, modalidade);

            return retorno
                    .Where(a => a != 0);
        }

        public async Task<IEnumerable<AbrangenciaTurmaRetorno>> ObterTurmas(string codigoUe, Modalidade modalidade)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            return await repositorioAbrangencia.ObterTurmas(codigoUe, login, perfil, modalidade);
        }

        public async Task<IEnumerable<AbrangenciaUeRetorno>> ObterUes(string codigoDre, Modalidade? modalidade)
        {
            var login = servicoUsuario.ObterLoginAtual();
            var perfil = servicoUsuario.ObterPerfilAtual();

            return await repositorioAbrangencia.ObterUes(codigoDre, login, perfil, modalidade);
        }
    }
}