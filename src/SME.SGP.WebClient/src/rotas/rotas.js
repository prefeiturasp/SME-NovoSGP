import React from 'react';
import { Switch, Route } from 'react-router-dom';
import history from '../servicos/history';
import { activeRoute, getRotas } from '../redux/modulos/navegacao/actions'
import { store } from '../redux';
import Principal from '../paginas/Principal/principal';
import PlanoCiclo from '../paginas/Planejamento/PlanoCiclo/planoCiclo';
import PlanoAnual from '../paginas/Planejamento/PlanoAnual/planoAnual';
import AtribuicaoSupervisorLista from '../paginas/Gestao/AtribuicaoSupervisor/atribuicaoSupervisorLista';
import AtribuicaoSupervisorCadastro from '../paginas/Gestao/AtribuicaoSupervisor/atribuicaoSupervisorCadastro';

export default function Rotas() {
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
    parent: '/gestao/atribuicao-supervisor-lista',
    component: AtribuicaoSupervisorCadastro,
    exact: true,
  });
  rotas.set('/gestao/atribuicao-supervisor/:dreId/:supervisorId', {
    breadcrumbName: 'Atribuição de Supervisor',
    parent: '/gestao/atribuicao-supervisor-lista',
    component: AtribuicaoSupervisorCadastro,
    exact: true,
  });

  const rotasParaRedux = new Map();

  history.listen((location) => {
    store.dispatch(activeRoute(location.pathname));
    localStorage.setItem('rota-atual', location.pathname);
  });

  const rotasArray = [];
  for (var [key, value] of rotas) {
    const rota = value;
    rota.path = key + (value.params ? value.params : '');
    rotasArray.push(rota);
    const rotaRedux = {
      params: value.params,
      breadcrumbName: value.breadcrumbName,
      parent: value.parent
    }
    rotasParaRedux.set(value.path, rotaRedux)
  }

  store.dispatch(getRotas(rotasParaRedux));

  return (
    <div>
      <Switch>
        {rotasArray.map(rota => {
          return (
            <Route
              path={rota.path}
              key={rota.path}
              exact={rota.exact}
              component={rota.component}
            />
          );
        })}
      </Switch>
    </div>
  );
}
