Ext.define('BarsProject.store.HumanStore', {
    extend: 'Ext.data.Store',
    model: 'BarsProject.model.Human',
    autoLoad: false,
    storeId: 'HumanStore',
    proxy: {
        type: 'ajax',
        url: '/home/SearchHuman',
        reader: {
            type: 'json',
            root: 'users'
        }
    }
});