import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import styled from 'styled-components';
import SelectComponent from '~/componentes/select';
import {
  setExpandirLinha,
  setNotaConceitoPosConselho,
} from '~/redux/modulos/conselhoClasse/actions';
import { erro } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import { CampoAlerta, CampoCentralizado } from './campoNotaConceito.css';

export const Combo = styled.div`
  width: 105px;
  height: 35px;
  display: inline-block;
`;

const CampoConceito = props => {
  const {
    notaPosConselho,
    listaTiposConceitos,
    id,
    idCampo,
    codigoComponenteCurricular,
  } = props;

  const [notaValorAtual, setNotaValorAtual] = useState(notaPosConselho);

  const dispatch = useDispatch();

  const mostrarJustificativa = () => {
    let novaLinha = {};
    novaLinha[idCampo] = true;
    dispatch(setExpandirLinha(novaLinha));
  };

  const setNotaPosConselho = (
    nota,
    ehEdicao,
    justificativa = null,
    auditoria = null
  ) => {
    dispatch(
      setNotaConceitoPosConselho({
        id,
        codigoComponenteCurricular,
        nota,
        ehEdicao,
        justificativa,
        auditoria,
        idCampo,
      })
    );
  };

  const onClickMostrarJustificativa = async () => {
    mostrarJustificativa();
    const dados = await ServicoConselhoClasse.obterNotaPosConselho(
      id
    ).catch(e => erro(e));
    if (dados && dados.data) {
      const { nota, justificativa } = dados.data;
      const auditoria = {
        criadoEm: dados.data.criadoEm,
        criadoPor: dados.data.criadoPor,
        criadoRf: dados.data.criadoRf,
        alteradoPor: dados.data.alteradoPor,
        alteradoEm: dados.data.alteradoEm,
        alteradoRf: dados.data.alteradoRf,
      };
      setNotaPosConselho(nota, false, justificativa, auditoria);
    }
  };

  const onChangeConceito = valorNovo => {
    setNotaValorAtual(String(valorNovo));
    mostrarJustificativa();
    setNotaPosConselho(valorNovo, true);
  };

  return (
    <>
      {id ? (
        <CampoCentralizado>
          <CampoAlerta>
            <Combo>
              <SelectComponent
                onChange={onChangeConceito}
                valueOption="id"
                valueText="valor"
                lista={listaTiposConceitos}
                valueSelect={notaValorAtual ? String(notaValorAtual) : ''}
                showSearch
                placeholder="Conceito"
              />
            </Combo>
            <div className="icone" onClick={onClickMostrarJustificativa}>
              <i className="fas fa-user-edit" />
            </div>
          </CampoAlerta>
        </CampoCentralizado>
      ) : (
        <Combo>
          <SelectComponent
            onChange={onChangeConceito}
            valueOption="id"
            valueText="valor"
            lista={listaTiposConceitos}
            valueSelect={notaValorAtual ? String(notaValorAtual) : ''}
            showSearch
            placeholder="Conceito"
          />
        </Combo>
      )}
    </>
  );
};

CampoConceito.propTypes = {
  notaPosConselho: PropTypes.oneOfType([PropTypes.any]),
  listaTiposConceitos: PropTypes.oneOfType([PropTypes.array]),
  id: PropTypes.oneOfType([PropTypes.number]),
  idCampo: PropTypes.oneOfType([PropTypes.string]),
  codigoComponenteCurricular: PropTypes.oneOfType([PropTypes.any]),
};

CampoConceito.defaultProps = {
  notaPosConselho: '',
  listaTiposConceitos: [],
  id: null,
  idCampo: '',
  codigoComponenteCurricular: '',
};

export default CampoConceito;
