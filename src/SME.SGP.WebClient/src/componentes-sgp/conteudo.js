import React, {useEffect, useState} from 'react';
import Rotas from '../rotas/rotas';
import { useSelector } from 'react-redux';

const Conteudo = () => {
  const MenuStore = useSelector(store => store.menu);
  const [collapsed, setCollapsed] = useState(false);

  useEffect(() => { setCollapsed(MenuStore.collapsed); }, [MenuStore.collapsed]);

  return (
    <main role="main" className={collapsed?"col-lg-11 col-md-10 col-sm-10 col-xs-12":"col-sm-8 col-md-9 col-lg-10"}>
      <div className="card-body m-r-0 m-l-0 p-l-0 p-r-0 m-t-0">
        <Rotas />
      </div>
    </main>
  );
}

export default Conteudo;
