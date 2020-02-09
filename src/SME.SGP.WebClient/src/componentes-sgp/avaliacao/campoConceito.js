import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import SelectComponent from '~/componentes/select';

const CampoConceito = props => {
  const { nota, onChangeNotaConceito, desabilitarCampo } = props;

  const [conceitoValorAtual, setConceitoValorAtual] = useState();
  const [conceitoAlterado, setConceitoAlterado] = useState(false);

  const modoEdicaoGeralNotaFinal = useSelector(
    store => store.notasConceitos.modoEdicaoGeralNotaFinal
  );

  const listaConceitos = [
    { valor: '1', descricao: 'P' },
    { valor: '2', descricao: 'S' },
    { valor: '3', descricao: 'NS' },
  ];

  const validaSeTeveAlteracao = (valorNovo, notaOriginal) => {
    if (notaOriginal) {
      setConceitoAlterado(valorNovo != notaOriginal);
    }
  };

  useEffect(() => {
    setConceitoValorAtual(nota.notaConceito);
    validaSeTeveAlteracao(nota.notaConceito, nota.notaOriginal);
  }, [nota.notaConceito, nota.notaOriginal]);

  const setarValorNovo = valorNovo => {
    setConceitoValorAtual(valorNovo);
    onChangeNotaConceito(valorNovo);
    validaSeTeveAlteracao(nota.notaConceito, nota.notaOriginal);
  };

  return (
    <SelectComponent
      onChange={valorNovo => setarValorNovo(valorNovo)}
      valueOption="valor"
      valueText="descricao"
      lista={listaConceitos}
      valueSelect={String(conceitoValorAtual) || undefined}
      showSearch
      placeholder="Conceito"
      className={`select-conceitos ${
        conceitoAlterado ? 'border-registro-alterado' : ''
      }`}
      classNameContainer={nota.ausente ? 'aluno-ausente-conceitos' : ''}
      disabled={
        desabilitarCampo || modoEdicaoGeralNotaFinal || !nota.podeEditar
      }
    />
  );
};

CampoConceito.defaultProps = {
  nota: {},
  onChangeNotaConceito: PropTypes.func,
  desabilitarCampo: PropTypes.bool,
};

CampoConceito.propTypes = {
  nota: {},
  onChangeNotaConceito: () => {},
  desabilitarCampo: false,
};

export default CampoConceito;
