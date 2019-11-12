import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import './configuracao/ReactotronConfig';
import history from './servicos/history';
import GlobalStyle from './estilos/global';
import { store } from './redux';
import { rotaAtiva } from './redux/modulos/navegacao/actions';
import Rotas from './rotas/rotas';

function App() {
  history.listen(location => {
    localStorage.setItem('rota-atual', location.pathname);
    store.dispatch(rotaAtiva(location.pathname));
  });

  return (
    <Provider store={store}>
      <Router history={history}>
        <GlobalStyle />
        <Rotas />
      </Router>
    </Provider>
  );
}

export default App;