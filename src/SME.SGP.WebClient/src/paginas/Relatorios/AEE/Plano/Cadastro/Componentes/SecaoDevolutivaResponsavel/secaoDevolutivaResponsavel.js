import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';

import { Button, Colors, Label } from '~/componentes';
import LocalizadorFuncionario from '~/componentes-sgp/LocalizadorFuncionario';

import { erros, history, sucesso } from '~/servicos';
import ServicoPlanoAEE from '~/servicos/Paginas/Relatorios/AEE/ServicoPlanoAEE';
import { RotasDto } from '~/dtos';

import {
  setDevolutivaEmEdicao,
  setDadosAtribuicaoResponsavel,
} from '~/redux/modulos/planoAEE/actions';

const SecaoDevolutivaResponsavel = () => {
  const [limparCampos, setLimparCampos] = useState(false);
  const [responsavelSelecionado, setResponsavelSelecionado] = useState();

  const dadosDevolutiva = useSelector(store => store.planoAEE.dadosDevolutiva);
  const planoAEEDados = useSelector(store => store.planoAEE.planoAEEDados);
  const dadosAtribuicaoResponsavel = useSelector(
    store => store.planoAEE.dadosAtribuicaoResponsavel
  );

  const dispatch = useDispatch();

  const onChangeLocalizador = funcionario => {
    setLimparCampos(false);
    if (funcionario?.codigoRF && funcionario?.nomeServidor) {
      const params = {
        codigoRF: funcionario?.codigoRF,
        nomeServidor: funcionario?.nomeServidor,
      };
      dispatch(setDadosAtribuicaoResponsavel(params));
      setResponsavelSelecionado(params);
      if (
        !dadosAtribuicaoResponsavel?.codigoRF &&
        !dadosDevolutiva?.responsavelRF
      ) {
        dispatch(setDevolutivaEmEdicao(true));
      }
    }
  };

  const onClickAtribuirResponsavel = async () => {
    const resposta = await ServicoPlanoAEE.atribuirResponsavel().catch(e =>
      erros(e)
    );

    if (resposta?.data) {
      history.push(RotasDto.RELATORIO_AEE_PLANO);
      sucesso('Atribuição do responsável realizada com sucesso');
    }
  };

  const onClickCancelar = () => {
    dispatch(setDadosAtribuicaoResponsavel({}));
    dispatch(setDevolutivaEmEdicao(false));
    setLimparCampos(true);
  };

  useEffect(() => {
    if (!dadosAtribuicaoResponsavel?.codigoRF) {
      setLimparCampos(true);
    }
  }, [dadosAtribuicaoResponsavel]);

  useEffect(() => {
    if (!dadosDevolutiva?.codigoRF) {
      setResponsavelSelecionado({
        codigoRF: dadosDevolutiva?.responsavelRF,
        nomeServidor: dadosDevolutiva?.responsavelNome,
      });
    }
  }, [dadosDevolutiva]);

  return (
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
            codigoRF: responsavelSelecionado?.codigoRF,
            nome: responsavelSelecionado?.nomeServidor,
          }}
          desabilitado={
            responsavelSelecionado?.codigoRF ||
            !dadosDevolutiva?.podeAtribuirResponsavel
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
            !responsavelSelecionado?.codigoRF ||
            !dadosDevolutiva?.podeAtribuirResponsavel
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
            !responsavelSelecionado?.codigoRF ||
            !dadosDevolutiva?.podeAtribuirResponsavel
          }
        />
      </div>
    </>
  );
};

export default SecaoDevolutivaResponsavel;
