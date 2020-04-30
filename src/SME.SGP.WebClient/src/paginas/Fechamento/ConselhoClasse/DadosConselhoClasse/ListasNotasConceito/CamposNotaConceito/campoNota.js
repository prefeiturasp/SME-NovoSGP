import PropTypes from 'prop-types';
import React, { useState } from 'react';
import CampoNumero from '~/componentes/campoNumero';
import {
  setExpandirLinha,
  setNotaConceitoPosConselho,
} from '~/redux/modulos/conselhoClasse/actions';
import { useDispatch } from 'react-redux';
import { CampoAlerta, CampoCentralizado } from './campoNota.css';
import { Tooltip } from 'antd';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import { erro } from '~/servicos/alertas';

const CampoNota = props => {
  const { id, notaPosConselho, idCampo, codigoComponenteCurricular } = props;

  const [notaValorAtual, setNotaValorAtual] = useState(notaPosConselho);

  const dispatch = useDispatch();

  const mostrarJustificativa = () => {
    let novaLinha = {};
    novaLinha[idCampo] = true;
    dispatch(setExpandirLinha(novaLinha));
  };

  const setNotaPosConselho = (
    nota,
    registroSalvo,
    justificativa = null,
    auditoria = null
  ) => {
    dispatch(
      setNotaConceitoPosConselho({
        id,
        codigoComponenteCurricular,
        nota,
        registroSalvo,
        justificativa,
        auditoria,
        idCampo,
      })
    );
  };

  const onChangeValor = valor => {
    setNotaValorAtual(valor);
    mostrarJustificativa();
    setNotaPosConselho(valor, false);
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
      setNotaPosConselho(nota, true, justificativa, auditoria);
    }
  };

  return (
    <>
      {id ? (
        <CampoCentralizado>
          <CampoAlerta>
            <CampoNumero
              onChange={onChangeValor}
              value={notaValorAtual}
              min={0}
              max={10}
              step={0.5}
            />
            <div className="icone">
              <Tooltip
                title="Teste"
                placement="bottom"
                overlayStyle={{ fontSize: '12px' }}
              >
                <i
                  className="fas fa-user-edit"
                  onClick={onClickMostrarJustificativa}
                ></i>
              </Tooltip>
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
};

CampoNota.defaultProps = {
  notaPosConselho: '',
};

export default CampoNota;
