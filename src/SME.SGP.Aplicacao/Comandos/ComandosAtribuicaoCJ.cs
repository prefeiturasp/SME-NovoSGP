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

            var professorValidoNoEol = await servicoEOL.ValidarProfessor(atribuicaoCJPersistenciaDto.UsuarioRf);
            if (!professorValidoNoEol)
                throw new NegocioException("Este professor não é válido para ser CJ.");

            var professoresTitularesDisciplinasEol = await servicoEOL.ObterProfessoresTitularesDisciplinas(atribuicaoCJPersistenciaDto.TurmaId);

            foreach (var atribuicaoDto in atribuicaoCJPersistenciaDto.Disciplinas)
            {
                var atribuicao = TransformaDtoEmEntidade(atribuicaoCJPersistenciaDto, atribuicaoDto);

                await servicoAtribuicaoCJ.Salvar(atribuicao, professoresTitularesDisciplinasEol, atribuicoesAtuais);

                Guid perfilCJ = atribuicao.Modalidade == Modalidade.Infantil ? Perfis.PERFIL_CJ_INFANTIL : Perfis.PERFIL_CJ;

                atribuiuCj = await AtribuirPerfilCJ(atribuicaoCJPersistenciaDto, perfilCJ, atribuiuCj);
            }
        }

        private async Task<bool> AtribuirPerfilCJ(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto, Guid perfil, bool atribuiuCj)
        {
            if (atribuiuCj)
                return atribuiuCj;

            await servicoEOL.AtribuirPerfil(atribuicaoCJPersistenciaDto.UsuarioRf, perfil);

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