import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import CardHeader from './cardHeader';
import CardBody from './cardBody';
import { Base } from './colors';

const CardCollapse = props => {
  const { titulo, indice, children } = props;

  const Card = styled.div`
    border-color: ${Base.CinzaDesabilitado} !important;

    &:last-child {
      margin-bottom: 0;
    }
  `;

  const Icon = styled.i`
    color: ${Base.CinzaBarras};
    &:hover {
    }
  `;

  const Link = styled.a`
    &[aria-expanded='true'] ${Icon} {
      color: ${Base.CinzaMako};
      transform: rotate(90deg);
    }
  `;

  return (
    <Card className="card shadow-sm mb-3">
      <CardHeader>
        {titulo}
        <Link
          className="text-decoration-none ml-auto"
          data-toggle="collapse"
          href={`#${indice}`}
          role="button"
          aria-expanded="false"
          aria-controls={`${indice}`}
        >
          <Icon className="fa fa-bars" aria-hidden="true" />
        </Link>
      </CardHeader>
      <div className="collapse fade" id={`${indice}`}>
        <CardBody>{children}</CardBody>
      </div>
    </Card>
  );
};

CardCollapse.propTypes = {
  titulo: PropTypes.string,
  indice: PropTypes.string,
  children: PropTypes.node,
};

export default CardCollapse;
