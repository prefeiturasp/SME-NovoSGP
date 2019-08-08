import React from 'react';
import { BrowserRouter, Switch, Route } from 'react-router-dom';

import Principal from '../pages/Principal/principal';
import Repositorio from '../pages/Repositorio/repositorio';

export default function Rotas() {
  return (
    <BrowserRouter>
      <Switch>
        <Route path="/" exact component={Principal} />
        <Route path="/repositorio" component={Repositorio} />
      </Switch>
    </BrowserRouter>
  );
}
