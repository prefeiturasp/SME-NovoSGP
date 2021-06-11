import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import { Base } from './colors';

const CardHeader = props => {
  const { indice, children, border, icon, show, onClick, configuracao } = props;

  const Header = styled.div`
    height: ${configuracao.altura};
    ${border
      ? `
      border-top-width: 0px !important;
      border-bottom-width: 0px !important;
      border-left: 8px solid ${configuracao.corBorda} !important;`
      : null}
    &.expanded {
      border-bottom-width: 1px !important;
    }
  `;

  const Icon = styled.i`
    color: ${Base.CinzaBarras} !important;
  `;

  const Link = styled.a`
    padding: 0.7rem 0.8rem !important;

    &:hover {
      background: ${Base.CinzaFundo} !important;
      border-radius: 50% !important;
    }

    &[aria-expanded='true'] ${Icon} {
      color: ${Base.CinzaMako} !important;
      transform: rotate(180deg) !important;
    }
  `;

  const handleHeader = event => {
    const header = event.target.parentElement.parentElement.classList;
    if (!header.contains('expanded')) header.add('expanded');
    else header.remove('expanded');

    if (onClick) onClick();
  };

  return (
    <Header
      className={`card-header shadow-sm rounded bg-white d-flex align-items-center ${show &&
        'expanded'} ${icon ? 'py-3' : 'py-4'} fonte-16`}
    >
      {children}
      {icon ? (
        <Link
          className="text-decoration-none ml-auto"
          data-toggle="collapse"
          href={`#${indice}`}
          id={`expandir-retrair-${indice}`}
          role="button"
          aria-expanded={show && true}
          aria-controls={`${indice}`}
          onClick={handleHeader}
        >
          <Icon className="fa fa-chevron-down" aria-hidden="true" />
        </Link>
      ) : null}
    </Header>
  );
};

CardHeader.propTypes = {
  indice: PropTypes.string,
  children: PropTypes.node,
  border: PropTypes.bool,
  icon: PropTypes.bool,
  show: PropTypes.bool,
  onClick: PropTypes.oneOfType([PropTypes.func]),
  configuracao: PropTypes.oneOfType([PropTypes.any]),
};

CardHeader.defaultProps = {
  indice: shortid.generate(),
  children: () => {},
  onClick: () => {},
  border: false,
  icon: false,
  show: false,
  configuracao: {
    altura: 'auto',
    corBorda: Base.AzulBordaCard,
  },
};

export default CardHeader;
