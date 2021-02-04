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
  const { changeLocalizadorResponsavel, clickCancelar } = props;
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
    if (funcionario?.rf && funcionario?.nome) {
      setFuncionarioLocalizadorSelecionado({
        rf: funcionario?.rf,
        nome: funcionario?.nome,
      });
    } else {
      setFuncionarioLocalizadorSelecionado();
      dispatch(setLimparDadosAtribuicaoResponsavel());
      changeLocalizadorResponsavel(funcionario);
    }
  };

  const onClickProximoPasso = () => {
    const params = {
      rf: funcionarioLocalizadorSelecionado.rf,
      nome: funcionarioLocalizadorSelecionado.nome,
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
      <div className="col-sm-12 col-md-12 col-lg-12 col-xl-9 mb-2">
        <div className="row">
          <LocalizadorFuncionario
            id="funcionario"
            onChange={onChangeLocalizador}
            valorInicial={funcionarioLocalizadorSelecionado?.codigoAluno}
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
          disabled={!funcionarioLocalizadorSelecionado?.codigoAluno}
        />
      </div>
    </div>
  );
};

CollapseAtribuicaoResponsavelDados.propTypes = {
  changeLocalizadorResponsavel: PropTypes.func,
  clickCancelar: PropTypes.func,
};

CollapseAtribuicaoResponsavelDados.defaultProps = {
  changeLocalizadorResponsavel: () => {},
  clickCancelar: () => {},
};

export default CollapseAtribuicaoResponsavelDados;
