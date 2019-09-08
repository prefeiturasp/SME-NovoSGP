import React, { useEffect, useState } from 'react';
import Rotas from '../rotas/rotas';
import { useSelector } from 'react-redux';

const Conteudo = () => {
  const NavegacaoStore = useSelector(store => store.navegacao);
  const [collapsed, setCollapsed] = useState(false);

  useEffect(() => { setCollapsed(NavegacaoStore.collapsed); }, [NavegacaoStore.collapsed]);

  return (
    <div className="row h-100" style={{ marginLeft: collapsed ? '115px' : '250px' }}>
      <main role="main" className="col-md-12 col-lg-12 col-sm-12 col-xl-12">
        <div className="card-body m-r-0 m-l-0 p-l-0 p-r-0 m-t-0">
          <Rotas />
        </div>
      </main>
    </div>
  );
}

export default Conteudo;
