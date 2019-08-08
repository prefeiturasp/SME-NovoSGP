import React from 'react';
import { Router } from 'react-router-dom';

import Rotas from './routes/index';
import history from './services/history';

import GlobalStyle from './styles/global';

function App() {
  return (
    <Router history={history}>
      <Rotas />
      <GlobalStyle />
    </Router>
  );
}

export default App;
