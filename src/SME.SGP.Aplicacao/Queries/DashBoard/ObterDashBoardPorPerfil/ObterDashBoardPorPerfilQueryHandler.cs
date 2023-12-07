using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Constantes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashBoardPorPerfilQueryHandler : IRequestHandler<ObterDashBoardPorPerfilQuery, IEnumerable<DashBoard>>
    {
        Guid[] perfil_1 = { Perfis.PERFIL_PROFESSOR_INFANTIL, Perfis.PERFIL_CJ_INFANTIL };
        Guid[] perfil_2 = { Perfis.PERFIL_PROFESSOR, Perfis.PERFIL_CJ, Perfis.PERFIL_POA, Perfis.PERFIL_PAEE, Perfis.PERFIL_PAP, Perfis.PERFIL_POEI, Perfis.PERFIL_POED, Perfis.PERFIL_POSL };

        private readonly IServicoUsuario servicoUsuario;

        public ObterDashBoardPorPerfilQueryHandler(IServicoUsuario servicoUsuario)
        {
            this.servicoUsuario = servicoUsuario ?? throw new ArgumentNullException(nameof(servicoUsuario));
        }

        public Task<IEnumerable<DashBoard>> Handle(ObterDashBoardPorPerfilQuery request, CancellationToken cancellationToken)
        {
            var perfilAtual = servicoUsuario.ObterPerfilAtual();
            var roles = servicoUsuario.ObterPermissoes();
            var listaDashBoard = new List<DashBoard>();

            if (perfil_1.Contains(perfilAtual))
            {
                listaDashBoard.Add(CarregaDashBoard(Permissao.CI_C, roles));
                listaDashBoard.Add(CarregaDashBoard(Permissao.DDB_C, roles));
                listaDashBoard.Add(CarregaDashBoard(Permissao.L_C, roles, descricao: "Listão"));
            }
            else if (perfil_2.Contains(perfilAtual))
            {
                listaDashBoard.Add(CarregaDashBoard(Permissao.CP_C, roles));
                listaDashBoard.Add(CarregaDashBoard(Permissao.PA_C, roles));

                if (perfilAtual == Perfis.PERFIL_PROFESSOR || perfilAtual == Perfis.PERFIL_CJ)
                    listaDashBoard.Add(CarregaDashBoard(Permissao.L_C, roles, descricao: "Listão"));
                else
                    listaDashBoard.Add(CarregaDashBoard(Permissao.PDA_C, roles));
            }
            else
            {
                listaDashBoard.Add(CarregaDashBoard(Permissao.PA_C, roles));
                listaDashBoard.Add(CarregaDashBoard(Permissao.C_C, roles, false));
                listaDashBoard.Add(CarregaDashBoard(Permissao.E_C, roles, false));
                if (perfilAtual == Perfis.PERFIL_ABAE)
                {
                    listaDashBoard.Add(CarregaDashBoard(Permissao.RABA_NAAPA_C, roles, false, ConstantesMenuPermissao.MENU_REGISTRO_ACOES));
                    listaDashBoard.Add(CarregaDashBoard(Permissao.CCEA_NAAPA_C, roles, false, ConstantesMenuPermissao.MENU_CONS_CRIANCAS_ESTUD_AUSENTES));
                }
            }

            return Task.FromResult<IEnumerable<DashBoard>>(listaDashBoard);

        }

        private DashBoard CarregaDashBoard(Permissao menu, IEnumerable<Permissao> roles, bool exigeTurma = true, string descricao = null)
        {
            var atributo = menu.GetAttribute<PermissaoMenuAttribute>();

            return new DashBoard()
            {
                Descricao = descricao ?? atributo.Menu,
                UsuarioTemPermissao = roles.Contains(menu),
                TurmaObrigatoria = exigeTurma,
                Icone = atributo.IconeDashBoard,
                Rota = atributo.Url,

            };
        }
    }
}
