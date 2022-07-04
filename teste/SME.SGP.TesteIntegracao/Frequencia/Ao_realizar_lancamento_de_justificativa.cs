using SME.SGP.TesteIntegracao.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.TesteIntegracao.Frequencia
{
    public class Ao_realizar_lancamento_de_justificativa : FrequenciaBase
    {
        public Ao_realizar_lancamento_de_justificativa(CollectionFixture collectionFixture) : base(collectionFixture)
        {

        }

        [Fact]
        public async Task Criar_justificativa_somente_com_motivo()
        {

        }
        [Fact]
        public async Task Criar_justificativa_somente_com_descricao()
        {

        }

        [Fact]
        public async Task Criar_justificativa_com_motivo_e_descricao()
        {

        }

        [Fact]
        public async Task Alterar_justificativa_com_motivo_e_descricao()
        {

        }

        [Fact]
        public async Task Alterar_justificativa_somente_com_descricao_sem_motivo()
        {

        }

        [Fact]
        public async Task Alterar_justificativa_somente_com_motivo_sem_descricao() 
        {

        }

        [Fact]
        public async Task Altear_justificativa_somente_motivo_com_descricao()
        {

        }

        [Fact]
        public async Task Alterar_justificativa_somente_descricao_com_motivo()
        {

        }

        [Fact]
        public async Task Excluir_justificativa()
        {

        }
    }
}
