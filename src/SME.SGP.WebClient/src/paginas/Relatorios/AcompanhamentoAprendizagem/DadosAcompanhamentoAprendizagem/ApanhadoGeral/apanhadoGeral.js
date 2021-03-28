import React from 'react';
import CardCollapse from '~/componentes/cardCollapse';
import AuditoriaApanhadoGeral from './auditoriaApanhadoGeral';
import CampoApanhadoGeral from './campoApanhadoGeral';

const ApanhadoGeral = () => {
  return (
    <CardCollapse
      key="apanhado-geral-collapse"
      titulo="Apanhado Geral"
      indice="apanhado-geral"
      alt="apanhado-geral"
    >
      <CampoApanhadoGeral />
      <AuditoriaApanhadoGeral />
    </CardCollapse>
  );
};

export default ApanhadoGeral;
