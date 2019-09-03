import React from 'react';
import { Link } from 'react-router-dom';
import Filtro from './filtro';

const Navbar = () => {
  return (
    <nav className="navbar navbar-expand-md navbar-light bg-white shadow-sm sticky-top">
      <Link className="navbar-brand col-sm-3 col-md-2 mr-0" to="/">
        SGP
      </Link>
      <Filtro />
    </nav>
  );
};

export default Navbar;
