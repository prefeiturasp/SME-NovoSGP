import React from 'react';
import { Provider } from 'react-redux';
import { Router, Switch } from 'react-router-dom';

import './configuracao/ReactotronConfig';
import { PersistGate } from 'redux-persist/integration/react';
import history from './servicos/history';
import GlobalStyle from './estilos/global';
import { store, persistor } from './redux';
import Pagina from '~/componentes-sgp/pagina';
import Login from '~/paginas/Login';
import RecuperarSenha from './paginas/RecuperarSenha';
import RedefinirSenha from './paginas/RedefinirSenha';
import RotaAutenticadaEstruturada from './rotas/rotaAutenticadaEstruturada';
import RotaNaoAutenticadaDesestruturada from './rotas/rotaNaoAutenticadaDesestruturada';
import RotaAutenticadaDesestruturada from './rotas/rotaAutenticadaDesestruturada';
import { rotaAtiva } from './redux/modulos/navegacao/actions';
import CapturaErros from './captura-erros';

function App() {
  history.listen(location => {
    localStorage.setItem('rota-atual', location.pathname);
    store.dispatch(rotaAtiva(location.pathname));
  });
  return (
    <Provider store={store}>
      <PersistGate loading={null} persistor={persistor}>
        <Router history={history}>
          <CapturaErros>
            <GlobalStyle />
            <div className="h-100">
              <Switch>
                <RotaAutenticadaDesestruturada
                  component={RedefinirSenha}
                  path="/redefinir-senha"
                />
                <RotaNaoAutenticadaDesestruturada
                  component={RedefinirSenha}
                  path="/redefinir-senha/:token"
                />
                <RotaNaoAutenticadaDesestruturada
                  component={RecuperarSenha}
                  path="/recuperar-senha"
                />
                <RotaNaoAutenticadaDesestruturada
                  component={Login}
                  path="/login/:redirect?/"
                />
                <RotaAutenticadaEstruturada component={Pagina} path="/" />
              </Switch>
            </div>
          </CapturaErros>
        </Router>
      </PersistGate>
    </Provider>
  );
}

export default App;
