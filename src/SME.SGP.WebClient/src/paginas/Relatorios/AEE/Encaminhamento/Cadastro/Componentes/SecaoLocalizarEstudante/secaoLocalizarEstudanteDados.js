import React, { useCallback, useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Loader, SelectComponent } from '~/componentes';
import { FiltroHelper } from '~/componentes-sgp';
import LocalizadorEstudante from '~/componentes/LocalizadorEstudante';
import { AbrangenciaServico, erros } from '~/servicos';
import Button from '~/componentes/button';
import { Colors } from '~/componentes/colors';
import { setDadosSecaoLocalizarEstudante } from '~/redux/modulos/encaminhamentoAEE/actions';

const SecaoLocalizarEstudanteDados = () => {
  const dispatch = useDispatch();

  const codigosAlunosSelecionados = useSelector(
    state => state.localizadorEstudante.codigosAluno
  );

  const dadosSecaoLocalizarEstudante = useSelector(
    store => store.encaminhamentoAEE.dadosSecaoLocalizarEstudante
  );

  const [anoAtual] = useState(window.moment().format('YYYY'));

  const listaAnosLetivo = [
    {
      desc: anoAtual,
      valor: anoAtual,
    },
  ];

  const [listaDres, setListaDres] = useState([]);
  const [listaUes, setListaUes] = useState([]);
  const [listaTurmas, setListaTurmas] = useState([]);

  const [codigoDre, setCodigoDre] = useState(
    dadosSecaoLocalizarEstudante?.codigoDre
  );
  const [codigoUe, setCodigoUe] = useState(
    dadosSecaoLocalizarEstudante?.codigoUe
  );
  const [codigoTurma, setCodigoTurma] = useState(
    dadosSecaoLocalizarEstudante?.codigoTurma
  );

  const [
    alunoLocalizadorSelecionado,
    setAlunoLocalizadorSelecionado,
  ] = useState(dadosSecaoLocalizarEstudante?.codigoAluno);

  const [carregandoTurmas, setCarregandoTurmas] = useState(false);
  const [carregandoDres, setCarregandoDres] = useState(false);

  useEffect(() => {
    if (codigosAlunosSelecionados?.length > 0) {
      setCodigoTurma();
    }
  }, [codigosAlunosSelecionados]);

  const [carregandoUes, setCarregandoUes] = useState(false);

  const obterUes = useCallback(async () => {
    if (codigoDre) {
      setCarregandoUes(true);
      const resposta = await AbrangenciaServico.buscarUes(
        codigoDre,
        `v1/abrangencias/false/dres/${codigoDre}/ues?anoLetivo=${anoAtual}`,
        true
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoUes(false));

      if (resposta?.data) {
        const lista = resposta.data.map(item => ({
          desc: item.nome,
          valor: String(item.codigo),
        }));

        if (lista?.length === 1) {
          setCodigoUe(lista[0].valor);
        }

        setListaUes(lista);
      } else {
        setListaUes([]);
      }
    }
  }, [codigoDre, anoAtual]);

  useEffect(() => {
    if (codigoDre) {
      obterUes();
    } else {
      setCodigoUe();
      setListaUes([]);
    }
  }, [codigoDre, obterUes]);

  const onChangeDre = dre => {
    setCodigoDre(dre);

    setListaUes([]);
    setCodigoUe();

    setListaTurmas([]);
    setCodigoTurma();
  };

  const obterDres = useCallback(async () => {
    if (anoAtual) {
      setCarregandoDres(true);
      const resposta = await AbrangenciaServico.buscarDres(
        `v1/abrangencias/false/dres?anoLetivo=${anoAtual}`
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoDres(false));

      if (resposta?.data?.length) {
        const lista = resposta.data
          .map(item => ({
            desc: item.nome,
            valor: String(item.codigo),
            abrev: item.abreviacao,
          }))
          .sort(FiltroHelper.ordenarLista('desc'));
        setListaDres(lista);

        if (lista && lista.length && lista.length === 1) {
          setCodigoDre(lista[0].valor);
        }
      } else {
        setListaDres([]);
        setCodigoDre();
      }
    }
  }, [anoAtual]);

  useEffect(() => {
    if (anoAtual) {
      obterDres();
    }
  }, [anoAtual, obterDres]);

  const obterTurmas = useCallback(async () => {
    if (codigoUe) {
      setCarregandoTurmas(true);
      const resposta = await AbrangenciaServico.buscarTurmas(
        codigoUe,
        0,
        '',
        anoAtual,
        false
      )
        .catch(e => erros(e))
        .finally(() => setCarregandoTurmas(false));

      if (resposta?.data) {
        setListaTurmas(resposta.data);

        if (resposta?.length === 1) {
          setCodigoTurma(resposta[0].valor);
        }
      }
    }
  }, [anoAtual, codigoUe]);

  useEffect(() => {
    if (codigoUe) {
      obterTurmas();
    } else {
      setCodigoTurma();
      setListaTurmas([]);
    }
  }, [codigoUe, obterTurmas]);

  const onChangeUe = ue => {
    setCodigoUe(ue);

    setListaTurmas([]);
    setCodigoTurma();
  };

  const onChangeTurma = valor => {
    setCodigoTurma(valor);
    setAlunoLocalizadorSelecionado();
  };

  const onChangeLocalizadorEstudante = aluno => {
    if (aluno?.alunoCodigo && aluno?.alunoNome) {
      setAlunoLocalizadorSelecionado(aluno.alunoCodigo);
    } else {
      setAlunoLocalizadorSelecionado();
    }
  };

  const onClickProximoPasso = async () => {
    // TODO
    console.log('onClickProximoPasso');

    const params = {
      anoLetivo: anoAtual,
      codigoDre,
      codigoUe,
      codigoTurma,
      codigoAluno: alunoLocalizadorSelecionado,
    };
    dispatch(setDadosSecaoLocalizarEstudante(params));
  };

  const onClickCancelar = async () => {
    // TODO
    console.log('onClickCancelar');
  };

  return (
    <div className="row">
      <div className="col-sm-12 col-md-6 col-lg-2 col-xl-2 mb-2">
        <SelectComponent
          id="ano-letivo"
          label="Ano Letivo"
          lista={listaAnosLetivo}
          valueOption="valor"
          valueText="desc"
          disabled
          valueSelect={anoAtual}
          placeholder="Ano letivo"
        />
      </div>
      <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
        <Loader loading={carregandoDres} tip="">
          <SelectComponent
            id="dre"
            label="Diretoria Regional de Educação (DRE)"
            lista={listaDres}
            valueOption="valor"
            valueText="desc"
            disabled={!anoAtual || listaDres?.length === 1}
            onChange={onChangeDre}
            valueSelect={codigoDre}
            placeholder="Diretoria Regional De Educação (DRE)"
          />
        </Loader>
      </div>
      <div className="col-sm-12 col-md-12 col-lg-5 col-xl-5 mb-2">
        <Loader loading={carregandoUes} tip="">
          <SelectComponent
            id="ue"
            label="Unidade Escolar (UE)"
            lista={listaUes}
            valueOption="valor"
            valueText="desc"
            disabled={!codigoDre || listaUes?.length === 1}
            onChange={onChangeUe}
            valueSelect={codigoUe}
            placeholder="Unidade Escolar (UE)"
          />
        </Loader>
      </div>
      <div className="col-sm-12 col-md-12 col-lg-12 col-xl-3 mb-2">
        <Loader loading={carregandoTurmas} tip="">
          <SelectComponent
            id="turma"
            lista={listaTurmas}
            valueOption="codigo"
            valueText="modalidadeTurmaNome"
            label="Turma"
            disabled={listaTurmas?.length === 1}
            valueSelect={codigoTurma}
            onChange={onChangeTurma}
            placeholder="Turma"
          />
        </Loader>
      </div>
      <div className="col-sm-12 col-md-12 col-lg-12 col-xl-9 mb-2">
        <div className="row">
          <LocalizadorEstudante
            id="estudante"
            showLabel
            ueId={codigoUe}
            onChange={onChangeLocalizadorEstudante}
            anoLetivo={anoAtual}
            desabilitado={!codigoDre || !codigoUe}
            codigoTurma={codigoTurma}
            valorInicialAlunoCodigo={alunoLocalizadorSelecionado}
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
        />
      </div>
    </div>
  );
};

export default SecaoLocalizarEstudanteDados;
