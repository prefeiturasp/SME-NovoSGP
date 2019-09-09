import React, { useEffect, useState } from 'react';
import { Breadcrumb } from 'antd';
import { Link } from 'react-router-dom';
import { useSelector } from 'react-redux';
import { Base } from '../componentes/colors';
import styled from 'styled-components';

const BreadcrumbSgp = (props) => {
    const BreadcrumbBody = styled.div`
        margin: 0 0 5px 25px;

        i{
            margin-right: 10px;
            margin-left: 10px;
        }
    `;

    const NavegacaoStore = useSelector(store => store.navegacao);

    const routes = new Map();
    routes.set('/', { breadcrumbName: 'Home', parent: null });
    routes.set('/planejamento/plano-ciclo', { breadcrumbName: 'Plano de Ciclo', menu: 'Planejamento' , parent: '/'});
    routes.set('/planejamento/plano-anual', { breadcrumbName: 'Plano Anual', menu: 'Planejamento',  parent: '/' });
    routes.set('/teste-filho', { breadcrumbName: 'Teste Filho',  parent: '/planejamento/plano-anual' });

    const [itens, setItens] = useState([]);

    useEffect(() => { carregaBreadcrumbs(NavegacaoStore.activeRoute); }, [NavegacaoStore.activeRoute]);

    const carregaBreadcrumbs = (route) => {
        const item = routes.get(route);
        if (item) {            
            const newItens = [];  
            carregaBreadcrumbsExtra(item, newItens);          
            newItens.push(criarItemBreadcrumb(item.breadcrumbName, route, true, true));
            setItens(newItens);
        }
    }

    const carregaBreadcrumbsExtra = (item, newItens) =>{        
        const itemParent = routes.get(item.parent);        
        if(itemParent && itemParent.parent){
            carregaBreadcrumbsExtra(itemParent, newItens);
          }
        if(item.parent){                
            newItens.push(criarItemBreadcrumb(itemParent.breadcrumbName, item.parent, false, false));
        }
        if(item.menu){
            newItens.push(criarItemBreadcrumb(item.menu, item.path+'-menu', true, false));
        }
    }

    const criarItemBreadcrumb = (breadcrumbName, path, ehEstatico, ehRotaAtual) => {
        return {breadcrumbName, path, ehEstatico, ehRotaAtual}
    }

    return (
        <BreadcrumbBody>
            {itens.map(item => {
                return (
                    <Breadcrumb.Item key={item.path} separator="">
                        <Link hidden={item.ehEstatico } to={item.path}>{item.breadcrumbName}</Link>
                        <span hidden={!item.ehEstatico}>{item.breadcrumbName}</span>
                        <i hidden={item.ehRotaAtual} style={{ color: Base.Roxo }} className='fas fa-chevron-circle-right' />
                    </Breadcrumb.Item>
                );
            })}
        </BreadcrumbBody>
    );
}

export default BreadcrumbSgp;