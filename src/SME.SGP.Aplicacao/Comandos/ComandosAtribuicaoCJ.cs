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
        private readonly IServicoAtribuicaoCJ servicoAtribuicaoCJ;
        private readonly IServicoEOL servicoEOL;

        public ComandosAtribuicaoCJ(IRepositorioAtribuicaoCJ repositorioAtribuicaoCJ, IServicoAtribuicaoCJ servicoAtribuicaoCJ, IServicoEOL servicoEOL)
        {
            this.repositorioAtribuicaoCJ = repositorioAtribuicaoCJ ?? throw new ArgumentNullException(nameof(repositorioAtribuicaoCJ));
            this.servicoAtribuicaoCJ = servicoAtribuicaoCJ ?? throw new ArgumentNullException(nameof(servicoAtribuicaoCJ));
            this.servicoEOL = servicoEOL ?? throw new ArgumentNullException(nameof(servicoEOL));
        }

        public async Task Salvar(AtribuicaoCJPersistenciaDto atribuicaoCJPersistenciaDto)
        {
            var atribuicoesAtuais = await repositorioAtribuicaoCJ.ObterPorFiltros(atribuicaoCJPersistenciaDto.Modalidade, atribuicaoCJPersistenciaDto.TurmaId,
               atribuicaoCJPersistenciaDto.UeId, 0, atribuicaoCJPersistenciaDto.UsuarioRf, string.Empty, null);

            bool atribuiuCj = false;

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

            return true;
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