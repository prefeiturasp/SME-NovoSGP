import { Tooltip } from 'antd';
import PropTypes from 'prop-types';
import React, { useCallback, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import CampoNumero from '~/componentes/campoNumero';
import {
  setExpandirLinha,
  setNotaConceitoPosConselho,
} from '~/redux/modulos/conselhoClasse/actions';
import { erro, erros } from '~/servicos/alertas';
import ServicoConselhoClasse from '~/servicos/Paginas/ConselhoClasse/ServicoConselhoClasse';
import ServicoNotaConceito from '~/servicos/Paginas/DiarioClasse/ServicoNotaConceito';
import { CampoAlerta, CampoCentralizado } from './campoNota.css';

const CampoNota = props => {
  const {
    id,
    notaPosConselho,
    idCampo,
    codigoComponenteCurricular,
    mediaAprovacao,
  } = props;

  const fechamentoPeriodoInicioFim = useSelector(
    store => store.conselhoClasse.fechamentoPeriodoInicioFim
  );

  const { periodoFechamentoFim } = fechamentoPeriodoInicioFim;

  const [notaValorAtual, setNotaValorAtual] = useState(notaPosConselho);
  const [abaixoDaMedia, setAbaixoDaMedia] = useState(false);

  const dispatch = useDispatch();

  const mostrarJustificativa = () => {
    const novaLinha = {};
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

  const removerCaracteresInvalidos = texto => {
    return texto.replace(/[^0-9,.]+/g, '');
  };

  const validaSeEstaAbaixoDaMedia = useCallback(
    valor => {
      const valorAtual = removerCaracteresInvalidos(String(valor));
      if (valorAtual && valorAtual < mediaAprovacao) {
        setAbaixoDaMedia(true);
      } else {
        setAbaixoDaMedia(false);
      }
    },
    [mediaAprovacao]
  );

  const onChangeValor = async (valor, validarMedia) => {
    setNotaValorAtual(valor);
    const retorno = await ServicoNotaConceito.obterArredondamento(
      valor,
      periodoFechamentoFim
    ).catch(e => erros(e));

    let notaArredondada = valor;
    if (retorno && retorno.data) {
      notaArredondada = retorno.data;
    }

    if (validarMedia) {
      validaSeEstaAbaixoDaMedia(notaArredondada);
    }
    mostrarJustificativa();
    setNotaPosConselho(valor, false);
    setNotaValorAtual(notaArredondada);
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

  const valorInvalido = valorNovo => {
    const regexValorInvalido = /[^0-9,.]+/g;
    return regexValorInvalido.test(String(valorNovo));
  };

  const editouCampo = (notaOriginal, notaNova) => {
    const novaNotaOriginal = removerCaracteresInvalidos(String(notaOriginal));
    const novaNotaNova = removerCaracteresInvalidos(String(notaNova));
    if (novaNotaOriginal === '' && novaNotaNova === '') {
      return false;
    }
    return novaNotaOriginal !== novaNotaNova;
  };

  const campoNotaPosConselho = (abaixoMedia, validarMedia) => {
    return (
      <CampoNumero
        onChange={valorNovo => {
          const invalido = valorInvalido(valorNovo);
          if (!invalido && editouCampo(notaValorAtual, valorNovo)) {
            onChangeValor(valorNovo, validarMedia);
          }
        }}
        value={notaValorAtual}
        min={0}
        max={10}
        step={0.5}
        className={abaixoMedia ? 'borda-abaixo-media' : ''}
      />
    );
  };

  return (
    <>
      <CampoCentralizado>
        {id ? (
          <CampoAlerta>
            {campoNotaPosConselho(false, false)}
            <div className="icone">
              <Tooltip
                title="Ver Justificativa"
                placement="bottom"
                overlayStyle={{ fontSize: '12px' }}
              >
                <i
                  className="fas fa-user-edit"
                  onClick={onClickMostrarJustificativa}
                />
              </Tooltip>
            </div>
          </CampoAlerta>
        ) : (
          <Tooltip
            placement="bottom"
            title={abaixoDaMedia ? 'Abaixo da Média' : ''}
          >
            <CampoCentralizado>
              {campoNotaPosConselho(abaixoDaMedia, true)}
            </CampoCentralizado>
          </Tooltip>
        )}
      </CampoCentralizado>
    </>
  );
};

CampoNota.propTypes = {
  id: PropTypes.oneOfType([PropTypes.any]),
  notaPosConselho: PropTypes.oneOfType([PropTypes.any]),
  idCampo: PropTypes.oneOfType([PropTypes.any]),
  codigoComponenteCurricular: PropTypes.string,
  mediaAprovacao: PropTypes.number,
};

CampoNota.defaultProps = {
  id: 0,
  notaPosConselho: '',
  idCampo: 0,
  codigoComponenteCurricular: '',
  mediaAprovacao: 5,
};

export default CampoNota;
