import React, { useState, createRef } from 'react';
import styled from 'styled-components';
import Grid from '../componentes/grid';
import Button from '../componentes/button';
import { Base, Colors } from '../componentes/colors';

const Filtro = () => {
  const [toggleBusca, setToggleBusca] = useState(false);
  const [modalidade, setModalidade] = useState();

  const Icon = styled.i`
    background: ${Base.CinzaDesabilitado} !important;
    border-radius: 50% !important;
    color: ${Base.CinzaMako} !important;
    cursor: pointer !important;
    padding: 0.7rem 0.9rem !important;
    right: 5px !important;
    top: 5px !important;
    ${toggleBusca && 'transform: rotate(180deg) !important;'}
  `;

  const FormGroup = styled.div`
    &:last-child {
      margin-bottom: 0 !important;
    }
  `;

  const FormRow = styled.div`
    &:last-child {
      margin-bottom: 0 !important;
    }
  `;

  const modalidadeRef = createRef();

  const mostraBusca = () => {
    setToggleBusca(!toggleBusca);
  };

  const selecionaModalidade = () => {
    setModalidade(modalidadeRef.current.value);
    modalidadeRef.current[modalidadeRef.current.selectedIndex].setAttribute(
      'selected',
      true
    );
  };

  return (
    <div className="position-relative w-50 mx-auto">
      <form>
        <FormGroup className="form-group mb-0 position-relative">
          <input
            type="text"
            className="form-control form-control-lg shadow-sm rounded"
          />
          <Icon
            className="fa fa-caret-down d-block position-absolute"
            onClick={mostraBusca}
          />
        </FormGroup>
        {toggleBusca && (
          <div className="container position-absolute bg-white shadow rounded px-3 pt-5 pb-1">
            <FormRow className="form-row">
              <Grid cols={2} className="form-group">
                <select className="form-control">
                  <option value="2019">2019</option>
                </select>
              </Grid>
              <Grid cols={modalidade === 'EJA' ? 5 : 10} className="form-group">
                <select
                  className="form-control"
                  onChange={selecionaModalidade}
                  ref={modalidadeRef}
                >
                  <option value="Fundamental">Ensino Fundamental</option>
                  <option value="EJA">EJA</option>
                </select>
              </Grid>
              {modalidade === 'EJA' && (
                <Grid cols={5} className="form-group">
                  <select
                    className="form-control"
                    onChange={selecionaModalidade}
                  >
                    <option value="1">1º Semestre</option>
                    <option value="2">2º Semestre</option>
                  </select>
                </Grid>
              )}
            </FormRow>
            <FormGroup className="form-group">
              <select className="form-control">
                <option value="IP">
                  Diretoria Regional de Educação (DRE) Ipiranga
                </option>
              </select>
            </FormGroup>
            <FormGroup className="form-group">
              <select className="form-control">
                <option value="1">
                  Unidade Escolar (UE) Luiz Gonzaga do Nascimento Jr -
                  Gonzaguinha
                </option>
              </select>
            </FormGroup>
            <FormRow className="form-row d-flex justify-content-between">
              <Grid cols={2} className="form-group">
                <select className="form-control">
                  <option>Turma</option>
                  <option value="1A">1-A</option>
                  <option value="1B">1-B</option>
                  <option value="1C">1-C</option>
                  <option value="1D">1-D</option>
                </select>
              </Grid>
              <Grid cols={3} className="form-group text-right">
                <Button label="Aplicar filtro" color={Colors.Roxo} />
              </Grid>
            </FormRow>
          </div>
        )}
      </form>
    </div>
  );
};

export default Filtro;
