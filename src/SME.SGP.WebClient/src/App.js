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
import { Deslogar } from '~/redux/modulos/usuario/actions';

function App() {

  window.addEventListener("beforeunload", function (event) {
    verificaSairResetSenha();
  });

  window.addEventListener('popstate', function (event) {
    if (performance.navigation.type == 1) {
      verificaSairResetSenha();
    }
  });

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
  }

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
