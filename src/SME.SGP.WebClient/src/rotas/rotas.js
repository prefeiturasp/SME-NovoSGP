import React, { createRef } from 'react';
import { Switch, Route } from 'react-router-dom';
import { useSelector } from 'react-redux';

export default function Rotas() {
  const NavegacaoStore = useSelector(store => store.navegacao);
  const rotas = NavegacaoStore.rotas;

  const rotasArray = [];
  for (var [key, value] of rotas) {
    const rota = value;
    rota.path = key;
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
