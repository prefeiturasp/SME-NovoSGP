import produce from 'immer';
import Principal from '../../../paginas/Principal/principal';
import PlanoCiclo from '../../../paginas/Planejamento/PlanoCiclo/planoCiclo';
import PlanoAnual from '../../../paginas/Planejamento/PlanoAnual/planoAnual';
import AtribuicaoSupervisorLista from '~/paginas/Gestao/AtribuicaoSupervisor/atribuicaoSupervisorLista';
import AtribuicaoSupervisorCadastro from '~/paginas/Gestao/AtribuicaoSupervisor/atribuicaoSupervisorCadastro';
import DetalheNotificacao from '~/paginas/Notificacoes/Detalhes/detalheNotificacao';

const rotas = new Map();
rotas.set('/', {
  params: ':rf?',
  breadcrumbName: 'Home',
  parent: null,
  component: Principal,
  exact: true,
  limpaSelecaoMenu: true
});
rotas.set('/planejamento/plano-ciclo', {
  breadcrumbName: 'Plano de Ciclo',
  menu: 'Planejamento',
  parent: '/',
  component: PlanoCiclo,
  exact: true,
});
rotas.set('/planejamento/plano-anual', {
  breadcrumbName: 'Plano Anual',
  menu: 'Planejamento',
  parent: '/',
  component: PlanoAnual,
  exact: false,
});
rotas.set('/gestao/atribuicao-supervisor-lista', {
  breadcrumbName: 'Atribuição de Supervisor',
  menu: 'Gestão',
  parent: '/',
  component: AtribuicaoSupervisorLista,
  exact: true,
});
rotas.set('/gestao/atribuicao-supervisor', {
  breadcrumbName: 'Atribuição de Supervisor',
  menu: 'Gestão',
  parent: '/',
  component: AtribuicaoSupervisorCadastro,
  exact: true,
});
rotas.set('/gestao/atribuicao-supervisor/:dreId/:supervisorId', {
  breadcrumbName: 'Atribuição de Supervisor',
  menu: 'Gestão',
  parent: '/',
  component: AtribuicaoSupervisorCadastro,
  exact: true,
});

rotas.set('/notificacoes/:id', {
  breadcrumbName: 'Notificações',
  parent: '/',
  component: DetalheNotificacao,
  exact: true,
});

const inicial = {
  collapsed: false,
  activeRoute: '/',
  rotas,
};

export default function navegacao(state = inicial, action) {
  return produce(state, draft => {
    switch (action.type) {
      case '@navegacao/collapsed':
        draft.collapsed = action.payload;
        break;
      case '@navegacao/activeRoute':
        draft.activeRoute = action.payload;
        break;
      case '@navegacao/rotas':
        draft.rotas.set(action.payload.path, action.payload);
        break;
      default:
        break;
    }
  });
}
