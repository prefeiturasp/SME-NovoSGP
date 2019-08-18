import React from 'react';
import { Router } from 'react-router-dom';

import Rotas from './rotas/rotas';
import history from './servicos/history';

import GlobalStyle from './estilos/global';
import Navbar from './componentes-sgp/navbar';
import Sidebar from './componentes-sgp/sidebar';

function App() {
  return (
    <Router history={history}>
      <GlobalStyle />
      <Navbar />
      <div className="container-fluid h-100">
        <div className="row h-100">
          <Sidebar />
          <main
            role="main"
            className="col-md-9 ml-sm-auto col-lg-10 bg-white pt-2"
          >
            <Rotas />
          </main>
        </div>
      </div>
    </Router>
  );
}

export default App;
