import React, { useEffect, useState } from 'react';
import PropTypes from 'prop-types';

import { useDispatch, useSelector } from 'react-redux';
import { Button, CardCollapse, Colors, Editor, Label } from '~/componentes';
import LocalizadorFuncionario from '~/componentes-sgp/LocalizadorFuncionario';

import { setLimparDadosAtribuicaoResponsavel } from '~/redux/modulos/collapseAtribuicaoResponsavel/actions';
import { verificaSomenteConsulta } from '~/servicos';
import { RotasDto, situacaoPlanoAEE } from '~/dtos';

const SecaoDevolutivasPlano = ({ match }) => {
  const [descricaoCordenacao, setDescricaoCordenacao] = useState();
  const [descricaoPAAI, setDescricaoPAAI] = useState();
  const [limparCampos, setLimparCampos] = useState(false);
  const [
    funcionarioLocalizadorSelecionado,
    setFuncionarioLocalizadorSelecionado,
  ] = useState();

  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);
  const usuario = useSelector(store => store.usuario);
  const permissoesTela = usuario.permissoes[RotasDto.RELATORIO_AEE_PLANO];
  const somenteConsulta = useSelector(store => store.navegacao.somenteConsulta);
  console.log('planoAEEDados', planoAEEDados);
  console.log('permissoesTela', permissoesTela);

  const situacaoDevolutivaCoordenacao =
    planoAEEDados.situacaoDescricao !== situacaoPlanoAEE.DevolutivaCoordenacao;
  const situacaoDevolutivaPAAI =
    planoAEEDados.situacaoDescricao !== situacaoPlanoAEE.DevolutivaPAAI;

  const dadosAtribuicaoResponsavel = useSelector(
    store => store.planoAEE.dadosAtribuicaoResponsavel
  );

  const dispatch = useDispatch();

  const mudarDescricaoCordenacao = texto => {
    setDescricaoCordenacao(texto);
  };

  const mudarDescricaoPAAI = texto => {
    setDescricaoPAAI(texto);
  };

  const onChangeLocalizador = funcionario => {
    setLimparCampos(false);
    if (funcionario?.codigoRF && funcionario?.nomeServidor) {
      setFuncionarioLocalizadorSelecionado({
        codigoRF: funcionario?.codigoRF,
        nomeServidor: funcionario?.nomeServidor,
      });
      return;
    }
    setFuncionarioLocalizadorSelecionado();
    dispatch(setLimparDadosAtribuicaoResponsavel());
  };

  const onClickAtribuirResponsavel = async () => {
    const params = {
      codigoRF: funcionarioLocalizadorSelecionado.codigoRF,
      nomeServidor: funcionarioLocalizadorSelecionado.nomeServidor,
    };
  };

  const onClickCancelar = () => {
    setFuncionarioLocalizadorSelecionado();
    dispatch(setLimparDadosAtribuicaoResponsavel());
    setLimparCampos(true);
  };

  useEffect(() => {
    verificaSomenteConsulta(permissoesTela);
  }, [permissoesTela]);

  return (
    <>
      <CardCollapse
        key="secao-devolutivas-plano-collapse-key"
        titulo="Devolutivas"
        show
        indice="secao-devolutivas-plano-collapse-indice"
        alt="secao-informacoes-plano-alt"
      >
        <div className="mb-3">
          <Editor
            label="Devolutiva da coordenação"
            onChange={mudarDescricaoCordenacao}
            inicial={descricaoCordenacao || ''}
            desabilitar={
              (somenteConsulta && !permissoesTela.podeIncluir) ||
              situacaoDevolutivaCoordenacao
            }
            removerToolbar={somenteConsulta && !permissoesTela.podeIncluir}
          />
        </div>

        {situacaoDevolutivaPAAI && (
          <>
            <Label text="Responsável" className="mb-3" />
            <div className="row">
              <LocalizadorFuncionario
                id="funcionario"
                onChange={onChangeLocalizador}
                codigoTurma={planoAEEDados?.turma?.codigo}
                limparCampos={limparCampos}
                url="v1/encaminhamento-aee/responsavel/pesquisa"
                valorInicial={{
                  codigoRF: dadosAtribuicaoResponsavel?.codigoRF,
                }}
                desabilitado={
                  situacaoDevolutivaPAAI ||
                  !!dadosAtribuicaoResponsavel?.codigoRF
                }
              />
            </div>
            <div className="col-12 d-flex justify-content-end pb-4 mt-2 pr-0">
              <Button
                id="btn-cancelar"
                label="Cancelar"
                color={Colors.Roxo}
                border
                className="mr-3"
                onClick={onClickCancelar}
                disabled={
                  !!dadosAtribuicaoResponsavel?.codigoRF ||
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
                  !!dadosAtribuicaoResponsavel?.codigoRF ||
                  !funcionarioLocalizadorSelecionado?.codigoRF
                }
              />
            </div>
          </>
        )}

        {!situacaoDevolutivaPAAI && (
          <div className="mb-3">
            <Editor
              label="Devolutiva do PAAI"
              onChange={mudarDescricaoPAAI}
              inicial={descricaoPAAI || ''}
              desabilitar={somenteConsulta && !permissoesTela.podeIncluir}
              removerToolbar={somenteConsulta && !permissoesTela.podeIncluir}
            />
          </div>
        )}
      </CardCollapse>
    </>
  );
};

SecaoDevolutivasPlano.defaultProps = {
  match: {},
};

SecaoDevolutivasPlano.propTypes = {
  match: PropTypes.oneOfType([PropTypes.object]),
};

export default SecaoDevolutivasPlano;
