Ext.define('BarsProject.store.HumanStore', {
    extend: 'Ext.data.Store',
    model: 'BarsProject.model.Human',
    autoLoad: false,
    pageSize: 100,
    storeId: 'HumanStore',
    proxy: {
        type: 'ajax',
        url: '/home/SearchHuman',
        reader: {
            type: 'json',
            root: 'humans',
            totalProperty: 'count'
        }
    }
});