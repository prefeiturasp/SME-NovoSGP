import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useDispatch } from 'react-redux';
import CampoNumero from '~/componentes/campoNumero';
import {
  setExpandirLinha,
  setNotaConceitoPosConselho,
} from '~/redux/modulos/conselhoClasse/actions';
import { erro } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import { CampoAlerta, CampoCentralizado } from './campoNotaConceito.css';

const CampoNota = props => {
  const { id, notaPosConselho, idCampo, codigoComponenteCurricular } = props;

  const [notaValorAtual, setNotaValorAtual] = useState(notaPosConselho);

  const dispatch = useDispatch();

  const mostrarJustificativa = () => {
    const novaLinha = {};
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

  const onChangeValor = valor => {
    setNotaValorAtual(valor);
    mostrarJustificativa();
    setNotaPosConselho(valor, true);
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

  return (
    <>
      {id ? (
        <CampoCentralizado>
          <CampoAlerta ehNota>
            <CampoNumero
              onChange={onChangeValor}
              value={notaValorAtual}
              min={0}
              max={10}
              step={0.5}
            />
            <div className="icone" onClick={onClickMostrarJustificativa}>
              <i className="fas fa-user-edit" />
            </div>
          </CampoAlerta>
        </CampoCentralizado>
      ) : (
        <CampoNumero
          onChange={onChangeValor}
          value={notaValorAtual}
          min={0}
          max={10}
          step={0.5}
        />
      )}
    </>
  );
};

CampoNota.propTypes = {
  notaPosConselho: PropTypes.oneOfType([PropTypes.any]),
  id: PropTypes.oneOfType([PropTypes.number]),
  idCampo: PropTypes.oneOfType([PropTypes.string]),
  codigoComponenteCurricular: PropTypes.oneOfType([PropTypes.any]),
};

CampoNota.defaultProps = {
  notaPosConselho: '',
  id: null,
  idCampo: '',
  codigoComponenteCurricular: '',
};

export default CampoNota;
