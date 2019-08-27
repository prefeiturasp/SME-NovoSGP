import React from 'react';
import Filtro from './filtro';

const Navbar = () => {
  return (
    <nav className="navbar navbar-expand-md navbar-light bg-white shadow-sm sticky-top">
      <a className="navbar-brand col-sm-3 col-md-2 mr-0" href="/">
        SGP
      </a>
      <Filtro />
    </nav>
  );
};

export default Navbar;
