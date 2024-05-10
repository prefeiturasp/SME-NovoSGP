import React from 'react';
import Navbar from '~/componentes-sgp/navbar/navbar';
import Sider from './sider';
import Conteudo from './conteudo';

const Pagina = () => {
  return (
    <>
      <Navbar />
      <div className="container-fluid h-100">
        <Sider />
        <Conteudo />
      </div>
    </>
  );
};

export default Pagina;
