import React from 'react';
import styled from 'styled-components';

export default function CardCollapse(props) {
  const Icone = styled.i``;

  const Link = styled.a`
    &[aria-expanded='true'] ${Icone} {
      transform: rotate(90deg);
    }
  `;

  const { titulo, indice } = props;

  return (
    <div className="card shadow-sm">
      <div className="card-header bg-white d-flex align-items-center">
        {titulo}
        <Link
          className="text-decoration-none cor-cinza ml-auto"
          data-toggle="collapse"
          data-target={`#${indice}`}
          aria-expanded="false"
          aria-controls={`${indice}`}
          href={`#${indice}`}
        >
          <Icone className="fa fa-bars stretched-link" aria-hidden="true" />
        </Link>
      </div>
      <div className="collapse fade" id={indice}>
        <div className="card-body p-0">Olar</div>
      </div>
    </div>
  );
}
