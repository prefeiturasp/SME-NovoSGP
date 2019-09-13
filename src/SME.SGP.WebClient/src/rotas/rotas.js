import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { setRotas } from '../redux/modulos/navegacao/actions'
import { store } from '../redux';
import Principal from '../paginas/Principal/principal';
import PlanoCiclo from '../paginas/Planejamento/PlanoCiclo/planoCiclo';
import PlanoAnual from '../paginas/Planejamento/PlanoAnual/planoAnual';
import AtribuicaoSupervisorLista from '../paginas/Gestao/AtribuicaoSupervisor/atribuicaoSupervisorLista';
import AtribuicaoSupervisorCadastro from '../paginas/Gestao/AtribuicaoSupervisor/atribuicaoSupervisorCadastro';

export default function Rotas(props) {
  const rotas = new Map();

  rotas.set('/:rf?', {
    breadcrumbName: 'Home',
    parent: null,
    component: Principal,
    exact: true,
    limpaSelecaoMenu: true,
    paginaInicial: true
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
    breadcrumbName: 'Nova Atribuição',
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

  const rotasArray = [];
  for (var [key, value] of rotas) {
    const rota = value;
    rota.path = key + (value.params ? value.params : '');
    rotasArray.push(rota);
    const rotaRedux = {
      path: value.paginaInicial?'/':key,
      params: value.params,
      breadcrumbName: value.breadcrumbName,
      menu: value.menu,
      parent: value.parent,
      limpaSelecaoMenu: value.limpaSelecaoMenu
    }
    store.dispatch(setRotas(rotaRedux));
  }


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
