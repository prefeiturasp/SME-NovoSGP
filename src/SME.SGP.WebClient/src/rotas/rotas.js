import React from 'react';
import { Switch, Route } from 'react-router-dom';
import { useSelector } from 'react-redux';
import history from '../servicos/history';
import { activeRoute } from '../redux/modulos/navegacao/actions'
import { store } from '../redux';

export default function Rotas() {
  const NavegacaoStore = useSelector(store => store.navegacao);
  const rotas = NavegacaoStore.rotas;


  history.listen((location) => {
    store.dispatch(activeRoute(location.pathname));
    localStorage.setItem('rota-atual', location.pathname);
  });

  const rotasArray = [];
  for (var [key, value] of rotas) {
    const rota = value;
    rota.path = key + (value.params ? value.params : '');
    rotasArray.push(rota);
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
