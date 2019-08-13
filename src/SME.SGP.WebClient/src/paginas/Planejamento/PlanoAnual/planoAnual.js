import React from 'react';
import shortid from 'shortid';
import styled from 'styled-components';
import CardCollapse from '../../../componentes/cardCollapse';
import Grid from '../../../componentes/grid';
import Button from '../../../componentes/button';

function selecionaMateria(event) {
  event.preventDefault();
  event.target.setAttribute(
    'aria-pressed',
    event.target.getAttribute('aria-pressed') === 'true' ? 'false' : 'true'
  );
}

function PlanoAnual() {
  const Icone = styled.i`
    color: rgba(0, 0, 0, 0.26);
  `;

  const Badge = styled.a`
    &:last-child {
      margin-right: 0;
    }
    &[aria-pressed='true'] {
      background: #f3f3f3 !important;
      border-color: #f3f3f3 !important;
    }
  `;

  const bimestres = [
    {
      nome: '1º Bimestre',
      materias: [
        { materia: 'Ciências' },
        { materia: 'História' },
        { materia: 'Geografia' },
      ],
    },
    { nome: '2º Bimestre', materias: [] },
    { nome: '3º Bimestre', materias: [] },
    { nome: '4º Bimestre', materias: [] },
  ];

  return (
    <div className="container">
      <div className="row">
        <Grid cols={12}>
          <h1>Plano Anual</h1>
        </Grid>
        <Grid cols={6} className="d-flex justify-content-start mb-3">
          <Button label="Migrar Conteúdo" disabled />
        </Grid>
        <Grid cols={6} className="d-flex justify-content-end mb-3">
          <Button label="Voltar" icon="arrow-left" className="mr-2" />
          <Button label="Salvar" disabled />
        </Grid>
        <Grid cols={12}>
          {bimestres.length > 0
            ? bimestres.map(bimestre => {
                const indice = shortid.generate().replace(/[0-9]/g, '');
                return (
                  <CardCollapse
                    key={indice}
                    titulo={bimestre.nome}
                    indice={indice}
                  >
                    <div className="row">
                      <Grid cols={6}>
                        <h6 className="d-inline-block font-weight-bold mb-3">
                          Objetivos de aprendizagem
                        </h6>
                        <Icone
                          className="fa fa-question-circle ml-2"
                          aria-hidden="true"
                        />
                        <div>
                          {bimestre.materias.length > 0
                            ? bimestre.materias.map(materia => {
                                return (
                                  <Badge
                                    role="button"
                                    href="#"
                                    onClick={selecionaMateria}
                                    aria-pressed={false}
                                    key={shortid.generate()}
                                    className="badge badge-pill border text-dark bg-white font-weight-light p-2 mr-2"
                                  >
                                    {materia.materia}
                                  </Badge>
                                );
                              })
                            : null}
                        </div>
                      </Grid>
                      <Grid cols={6}>
                        <strong>
                          Objetivos de aprendizagem e meus objetivos (Currículo
                          da cidade)
                        </strong>
                      </Grid>
                    </div>
                  </CardCollapse>
                );
              })
            : null}
        </Grid>
      </div>
    </div>
  );
}

export default PlanoAnual;
