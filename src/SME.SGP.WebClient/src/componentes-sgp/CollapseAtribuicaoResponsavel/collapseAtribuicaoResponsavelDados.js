import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useDispatch } from 'react-redux';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import {
  setDadosCollapseAtribuicaoResponsavel,
  setLimparDadosAtribuicaoResponsavel,
} from '~/redux/modulos/collapseAtribuicaoResponsavel/actions';
import LocalizadorFuncionario from '../LocalizadorFuncionario';

const CollapseAtribuicaoResponsavelDados = props => {
  const {
    validarAntesProximoPasso,
    changeLocalizadorResponsavel,
    clickCancelar,
    codigoTurma,
    url,
  } = props;
  const dispatch = useDispatch();

  const [
    funcionarioLocalizadorSelecionado,
    setFuncionarioLocalizadorSelecionado,
  ] = useState();

  useEffect(() => {
    return () => dispatch(setLimparDadosAtribuicaoResponsavel({}));
  }, [dispatch]);

  const onChangeLocalizador = funcionario => {
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

  const onClickProximoPasso = async () => {
    const params = {
      codigoRF: funcionarioLocalizadorSelecionado.codigoRF,
      nomeServidor: funcionarioLocalizadorSelecionado.nomeServidor,
    };

    let continuar = true;
    if (validarAntesProximoPasso) {
      continuar = await validarAntesProximoPasso(params);
    }

    if (continuar) {
      dispatch(setDadosCollapseAtribuicaoResponsavel(params));
    }
  };

  const onClickCancelar = () => {
    setFuncionarioLocalizadorSelecionado();
    dispatch(setLimparDadosAtribuicaoResponsavel());

    clickCancelar();
  };

  return (
    <div className="row">
      <div className="col-md-12 mb-2">
        <div className="row">
          <LocalizadorFuncionario
            id="funcionario"
            onChange={onChangeLocalizador}
            codigoTurma={codigoTurma}
            url={url}
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
        />
        <Button
          id="btn-proximo-passo"
          label="Próximo passo"
          color={Colors.Roxo}
          border
          bold
          onClick={onClickProximoPasso}
          disabled={!funcionarioLocalizadorSelecionado?.codigoRF}
        />
      </div>
    </div>
  );
};

CollapseAtribuicaoResponsavelDados.propTypes = {
  validarAntesProximoPasso: PropTypes.func,
  changeLocalizadorResponsavel: PropTypes.func,
  clickCancelar: PropTypes.func,
  codigoTurma: PropTypes.string,
  url: PropTypes.string,
};

CollapseAtribuicaoResponsavelDados.defaultProps = {
  validarAntesProximoPasso: null,
  changeLocalizadorResponsavel: () => {},
  clickCancelar: () => {},
  codigoTurma: '',
  url: '',
};

export default CollapseAtribuicaoResponsavelDados;
