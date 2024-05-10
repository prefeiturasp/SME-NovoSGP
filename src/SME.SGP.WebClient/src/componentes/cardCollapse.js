import React from 'react';
import styled from 'styled-components';
import PropTypes from 'prop-types';
import shortid from 'shortid';
import CardHeader from './cardHeader';
import CardBody from './cardBody';
import { Base } from './colors';

const CardCollapse = React.forwardRef((props, ref) => {
  const {
    titulo,
    indice,
    children,
    show,
    onClick,
    configCabecalho,
    icon,
    styleCardBody,
  } = props;

  const Card = styled.div`
    border-color: ${Base.CinzaDesabilitado} !important;

    &:last-child {
      margin-bottom: 0 !important;
    }
  `;

  return (
    <Card ref={ref} className="card shadow-sm mb-3">
      <CardHeader
        indice={indice}
        border
        icon={icon}
        show={show}
        onClick={onClick}
        configuracao={configCabecalho}
      >
        {titulo}
      </CardHeader>
      <div className={`collapse fade ${show && 'show'}`} id={`${indice}`}>
        <CardBody style={styleCardBody}>{children}</CardBody>
      </div>
    </Card>
  );
});

CardCollapse.propTypes = {
  titulo: PropTypes.oneOfType([PropTypes.any]),
  indice: PropTypes.string,
  children: PropTypes.node,
  show: PropTypes.bool,
  onClick: PropTypes.oneOfType([PropTypes.func]),
  configCabecalho: PropTypes.oneOfType([PropTypes.any]),
  icon: PropTypes.bool,
  styleCardBody: PropTypes.oneOfType([PropTypes.any]),
};

CardCollapse.defaultProps = {
  titulo: '',
  indice: shortid.generate(),
  children: () => {},
  onClick: () => {},
  show: false,
  configCabecalho: {
    altura: 'auto',
    corBorda: Base.AzulBordaCard,
  },
  icon: true,
  styleCardBody: null,
};

export default CardCollapse;
