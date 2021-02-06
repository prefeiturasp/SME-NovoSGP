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
  const { changeLocalizadorResponsavel, clickCancelar, codigoTurma } = props;
  const dispatch = useDispatch();

  const [
    funcionarioLocalizadorSelecionado,
    setFuncionarioLocalizadorSelecionado,
  ] = useState();

  useEffect(() => {
    return () => dispatch(setLimparDadosAtribuicaoResponsavel({}));
  }, [dispatch]);

  const onChangeLocalizador = funcionario => {
    // TODO
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

  const onClickProximoPasso = () => {
    const params = {
      codigoRF: funcionarioLocalizadorSelecionado.codigoRF,
      nomeServidor: funcionarioLocalizadorSelecionado.nomeServidor,
    };

    dispatch(setDadosCollapseAtribuicaoResponsavel(params));
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
            // valorInicial={funcionarioLocalizadorSelecionado?.codigoRF}
            codigoTurma="2257361"
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
  changeLocalizadorResponsavel: PropTypes.func,
  clickCancelar: PropTypes.func,
  codigoTurma: PropTypes.string,
};

CollapseAtribuicaoResponsavelDados.defaultProps = {
  changeLocalizadorResponsavel: () => {},
  clickCancelar: () => {},
  codigoTurma: '2257361',
};

export default CollapseAtribuicaoResponsavelDados;
