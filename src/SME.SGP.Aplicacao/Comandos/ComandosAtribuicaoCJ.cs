using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ComandosAtribuicaoCJ : IComandosAtribuicaoCJ
    {
        private readonly IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ;
        private readonly IRepositorioCache repositorioCache;
        private readonly IServicoAtribuicaoCJ servicoAtribuicaoCJ;
        private readonly IServicoEol servicoEOL;
        private readonly IServicoUsuario servicoUsuario;

        public ComandosAtribuicaoCJ(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ, IServicoAtribuicaoCJ servicoAtribuicaoCJ,
            IServicoEol servicoEOL, IServicoUsuario servicoUsuario, IRepositorioCache repositorioCache)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.servicoAtribuicaoCJ = servicoAtribuicaoCJ ?? throw new ArgumentNullException(nameof(servicoAtribuicaoCJ));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
            this.repositorioCache = repositorioCache ?? throw new ArgumentNullException(nameof(repositorioCache));
        }

        public async Task Salvar(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto)
        {
            var atribuicoesAtuais = await repositorioAtribuicaoCJ.ObterPorFiltros(atribuicaoCJPersistenciaDto.Modalidade, atribuicaoCJPersistenciaDto.TurmaId,
               atribuicaoCJPersistenciaDto.UeId, 0, atribuicaoCJPersistenciaDto.UsuarioRf, string.Empty, null);

            bool atribuiuCj = false;

            await RemoverDisciplinasCache(atribuicaoCJPersistenciaDto);

            foreach (var atribuicaoDto in atribuicaoCJPersistenciaDto.Disciplinas)
            {
                var atribuicao = TransformaDtoEmEntidade(atribuicaoCJPersistenciaDto, atribuicaoDto);

                await servicoAtribuicaoCJ.Salvar(atribuicao, atribuicoesAtuais);

                atribuiuCj = await AtribuirPerfilCJ(atribuicaoCJPersistenciaDto, atribuiuCj);
            }
        }

        private async Task<bool> AtribuirPerfilCJ(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto, bool atribuiuCj)
        {
            if (atribuiuCj)
                return atribuiuCj;

            await servicoEOL.AtribuirCJSeNecessario(atribuicaoCJPersistenciaDto.UsuarioRf);

            servicoUsuario.RemoverPerfisUsuarioAtual();

            return true;
        }

        private async Task RemoverDisciplinasCache(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto)
        {
            var chaveCache = $"Disciplinas-{atribuicaoCJPersistenciaDto.TurmaId}-{atribuicaoCJPersistenciaDto.UsuarioRf}--{Perfis.PERFIL_CJ}";
            await repositorioCache.RemoverAsync(chaveCache);
        }

        private AtribuicaoCJ TransformaDtoEmEntidade(AtribuicaoCJPersistenciaDto dto, AtribuicaoCJPersistenciaItemDto itemDto)
        {
            return new AtribuicaoCJ()
            {
                DreId = dto.DreId,
                Modalidade = dto.Modalidade,
                ProfessorRf = dto.UsuarioRf,
                Substituir = itemDto.Substituir,
                TurmaId = dto.TurmaId,
                UeId = dto.UeId,
                DisciplinaId = itemDto.DisciplinaId
            };
        }
    }
}