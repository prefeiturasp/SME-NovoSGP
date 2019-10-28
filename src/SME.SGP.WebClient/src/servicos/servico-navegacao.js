import api from '~/servicos/api';
import {store} from '~/redux';
import {setMenu} from '~/redux/modulos/usuario/actions';

const setMenus = () => {
    api.get('v1/menus').then(resp => {
        store.dispatch(setMenu(resp.data));
    });
}

export {setMenus};