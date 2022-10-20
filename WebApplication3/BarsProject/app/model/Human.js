Ext.define('BarsProject.model.Human', {
    extend: 'Ext.data.Model',
    fields: [{
        name: 'id',
        type: 'int'
    }, {
        name: 'surname',
        type: 'string'
    }, {
        name: 'fname',
        type: 'string'
    }, {
        name: 'patronymic',
        type: 'string'
    }, {
        name: 'birthday',
        type: 'date'
    }]
});  