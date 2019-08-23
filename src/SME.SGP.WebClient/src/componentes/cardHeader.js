import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import { Base } from './colors';

const CardHeader = props => {
  const { indice, children, border, icon, show } = props;

  const Header = styled.div`
    ${border
      ? `
      border-top-width: 0px !important;
      border-bottom-width: 0px !important;
      border-left: 8px solid ${Base.AzulBordaCard} !important;`
      : null}
  `;

  const Icon = styled.i`
    color: ${Base.CinzaBarras};
  `;

  const Link = styled.a`
    padding: .5rem .6rem !important;

    &:hover {
      background: ${Base.CinzaFundo};
      border-radius: 50%;
    }

    &[aria-expanded='true'] ${Icon} {
      color: ${Base.CinzaMako};
      transform: rotate(180deg);
    }

    &[aria-expanded='true'] ${Header} {
      border-bottom-width: 1px !important;
    }
  `;

  return (
    <Header
      className={`card-header shadow-sm rounded bg-white d-flex align-items-center ${
        icon ? 'py-3' : 'py-4'
      } fonte-16`}
    >
      {children}
      {icon ? (
        <Link
          className="text-decoration-none ml-auto"
          data-toggle="collapse"
          href={`#${indice}`}
          role="button"
          aria-expanded={show && true}
          aria-controls={`${indice}`}
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
};

CardHeader.defaultProps = {
  show: false,
};

export default CardHeader;
