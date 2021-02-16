import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import {
  setDadosCollapseAtribuicaoResponsavel,
  setLimparDadosAtribuicaoResponsavel,
} from '~/redux/modulos/collapseAtribuicaoResponsavel/actions';
import LocalizadorFuncionario from '../LocalizadorFuncionario';

const CollapseAtribuicaoResponsavelDados = props => {
  const {
    validarAntesAtribuirResponsavel,
    changeLocalizadorResponsavel,
    clickCancelar,
    codigoTurma,
    url,
    clickRemoverResponsavel,
  } = props;
  const dispatch = useDispatch();

  const dadosCollapseAtribuicaoResponsavel = useSelector(
    store =>
      store.collapseAtribuicaoResponsavel.dadosCollapseAtribuicaoResponsavel
  );

  const [
    funcionarioLocalizadorSelecionado,
    setFuncionarioLocalizadorSelecionado,
  ] = useState();

  const [limparCampos, setLimparCampos] = useState(false);

  const onChangeLocalizador = funcionario => {
    setLimparCampos(false);
    if (funcionario?.codigoRF && funcionario?.nomeServidor) {
      setFuncionarioLocalizadorSelecionado({
        codigoRF: funcionario?.codigoRF,
        nomeServidor: funcionario?.nomeServidor,
      });
    } else {
      setFuncionarioLocalizadorSelecionado();
      dispatch(setLimparDadosAtribuicaoResponsavel());
      changeLocalizadorResponsavel(funcionario);
    }
  };

  const onClickAtribuirResponsavel = async () => {
    const params = {
      codigoRF: funcionarioLocalizadorSelecionado.codigoRF,
      nomeServidor: funcionarioLocalizadorSelecionado.nomeServidor,
    };

    let continuar = true;
    if (validarAntesAtribuirResponsavel) {
      continuar = await validarAntesAtribuirResponsavel(params);
    }

    if (continuar) {
      dispatch(setDadosCollapseAtribuicaoResponsavel(params));
    }
  };

  const onClickCancelar = () => {
    setFuncionarioLocalizadorSelecionado();
    dispatch(setLimparDadosAtribuicaoResponsavel());
    setLimparCampos(true);
    clickCancelar();
  };

  const onClickRemover = () => {
    if (clickRemoverResponsavel) {
      clickRemoverResponsavel(funcionarioLocalizadorSelecionado);
    }
  };

  return (
    <div className="row">
      <div className="col-md-12 mb-2">
        <div className="row">
          <LocalizadorFuncionario
            id="funcionario"
            onChange={onChangeLocalizador}
            codigoTurma={codigoTurma}
            limparCampos={limparCampos}
            url={url}
            valorInicial={{
              codigoRF: dadosCollapseAtribuicaoResponsavel?.codigoRF,
            }}
            desabilitado={!!dadosCollapseAtribuicaoResponsavel?.codigoRF}
          />
        </div>
      </div>
      <div className="col-md-12 d-flex justify-content-end pb-4 mt-2">
        <Button
          id="btn-cancelar"
          label="Cancelar"
          color={Colors.Roxo}
          border
          className="mr-3"
          onClick={onClickCancelar}
          disabled={
            !!dadosCollapseAtribuicaoResponsavel?.codigoRF ||
            !funcionarioLocalizadorSelecionado?.codigoRF
          }
        />
        <Button
          id="btn-atribuir"
          label="Atribuir responsável"
          color={Colors.Roxo}
          border
          bold
          onClick={onClickAtribuirResponsavel}
          disabled={
            !!dadosCollapseAtribuicaoResponsavel?.codigoRF ||
            !funcionarioLocalizadorSelecionado?.codigoRF
          }
        />
        <Button
          id="btn-remover"
          label="Remover responsável"
          color={Colors.Roxo}
          border
          bold
          className="ml-3"
          onClick={onClickRemover}
          disabled={!dadosCollapseAtribuicaoResponsavel?.codigoRF}
        />
      </div>
    </div>
  );
};

CollapseAtribuicaoResponsavelDados.propTypes = {
  validarAntesAtribuirResponsavel: PropTypes.func,
  changeLocalizadorResponsavel: PropTypes.func,
  clickCancelar: PropTypes.func,
  codigoTurma: PropTypes.string,
  url: PropTypes.string,
  clickRemoverResponsavel: PropTypes.func,
};

CollapseAtribuicaoResponsavelDados.defaultProps = {
  validarAntesAtribuirResponsavel: null,
  changeLocalizadorResponsavel: () => {},
  clickCancelar: () => {},
  codigoTurma: '',
  url: '',
  clickRemoverResponsavel: null,
};

export default CollapseAtribuicaoResponsavelDados;
