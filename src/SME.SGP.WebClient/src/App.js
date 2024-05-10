import React, { useEffect } from 'react';

import { Provider } from 'react-redux';
import { Router, Switch } from 'react-router-dom';
import ReactGA from 'react-ga';
import { obterTrackingID } from './servicos/variaveis';

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
import { Deslogar } from '~/redux/modulos/usuario/actions';
import VersaoSistema from '~/componentes-sgp/VersaoSistema';

obterTrackingID().then(id => ReactGA.initialize(id));

function App() {
  const verificaSairResetSenha = () => {
    const persistJson = localStorage.getItem('persist:sme-sgp');
    if (persistJson) {
      const dados = JSON.parse(persistJson);
      if (dados && dados.usuario) {
        const usuario = JSON.parse(dados.usuario);
        if (usuario && usuario.logado && usuario.modificarSenha) {
          store.dispatch(Deslogar());
        }
      }
    }
  };

  window.addEventListener('beforeunload', () => {
    verificaSairResetSenha();
  });

  window.addEventListener('popstate', () => {
    if (performance.navigation.type === 1) {
      verificaSairResetSenha();
    }
  });

  history.listen(location => {
    localStorage.setItem('rota-atual', location.pathname);
    store.dispatch(rotaAtiva(location.pathname));
    ReactGA.set({ page: location.pathname });
    ReactGA.pageview(location.pathname);
  });

  return (
    <Provider store={store}>
      <PersistGate loading={null} persistor={persistor}>
        <Router history={history}>
          <CapturaErros>
            <GlobalStyle />
            <VersaoSistema />
            <div className="h-100">
              <Switch>
                <RotaNaoAutenticadaDesestruturada
                  component={RedefinirSenha}
                  path="/redefinir-senha/:token"
                  exact
                />
                <RotaAutenticadaDesestruturada
                  component={RedefinirSenha}
                  path="/redefinir-senha"
                  exact
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
