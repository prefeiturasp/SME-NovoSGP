import React from 'react';
import { Provider } from 'react-redux';
import { Router, Switch, Route } from 'react-router-dom';

import './configuracao/ReactotronConfig';
import { PersistGate } from 'redux-persist/integration/react';
import history from './servicos/history';
import GlobalStyle from './estilos/global';
import { store, persistor } from './redux';
// import { rotaAtiva } from './redux/modulos/navegacao/actions';
import Pagina from '~/componentes-sgp/pagina';
import Login from '~/paginas/Login';

import Rotas from './rotas/rotas';
import Navbar from './componentes-sgp/navbar';

function App() {
  // history.listen(location => {
  //   localStorage.setItem('rota-atual', location.pathname);
  //   store.dispatch(rotaAtiva(location.pathname));
  // });
  return (
    <Provider store={store}>
      <PersistGate loading={null} persistor={persistor}>
        <Router history={history}>
          <GlobalStyle />
          <div className="h-100">
            <Switch>
              <Route component={Login} path="/login/:redirect?/" />
              <Route component={Pagina} path="/" />
            </Switch>
          </div>
        </Router>
      </PersistGate>
    </Provider>
  );
}

export default App;
