import React from 'react';
import { Router } from 'react-router-dom';

import Rotas from './rotas/rotas';
import history from './servicos/history';

import GlobalStyle from './estilos/global';

function App() {
  return (
    <Router history={history}>
      <Rotas />
      <GlobalStyle />
    </Router>
  );
}

export default App;
