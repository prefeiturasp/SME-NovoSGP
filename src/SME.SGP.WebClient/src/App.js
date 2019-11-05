import React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import './configuracao/ReactotronConfig';
import history from './servicos/history';
import GlobalStyle from './estilos/global';
import { store, persistor } from './redux';
import { rotaAtiva } from './redux/modulos/navegacao/actions';
import { PersistGate } from 'redux-persist/integration/react';
import Rotas from './rotas/rotas';

function App() {
  history.listen(location => {
    localStorage.setItem('rota-atual', location.pathname);
    store.dispatch(rotaAtiva(location.pathname));
  });

  return (
    <Provider store={store}>
      <PersistGate loading={null} persistor={persistor}>
        <Router history={history}>
          <GlobalStyle />
          <Rotas />
        </Router>
      </PersistGate>
    </Provider>
  );
}

export default App;
