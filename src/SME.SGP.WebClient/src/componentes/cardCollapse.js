import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';

const CardCollapse = props => {
  const Icone = styled.i``;

  const Link = styled.a`
    &[aria-expanded='true'] ${Icone} {
      transform: rotate(90deg);
    }
  `;

  const { titulo, indice } = props;

  return (
    <div className="card shadow-sm mb-2">
      <div className="card-header bg-white d-flex align-items-center">
        {titulo}
        <Link
          className="text-decoration-none ml-auto"
          data-toggle="collapse"
          href={`#${indice}`}
          role="button"
          aria-expanded="false"
          aria-controls={`${indice}`}
        >
          <Icone className="fa fa-bars stretched-link" aria-hidden="true" />
        </Link>
      </div>
      <div className="collapse fade" id={`${indice}`}>
        <div className="card-body p-0">Olar</div>
      </div>
    </div>
  );
};

CardCollapse.propTypes = {
  titulo: PropTypes.string,
  indice: PropTypes.string,
};

export default CardCollapse;
