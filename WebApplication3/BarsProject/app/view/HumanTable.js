Ext.define('BarsProject.view.HumanTable' ,{
    extend: 'Ext.grid.Panel',
    alias: 'widget.humantable',
    store: 'BarsProject.store.HumanStore',

    initComponent: function() {
        this.columns = [{
            header: 'Фамилия',
            dataIndex: 'surname',
            flex:1
        }, {
            header: 'Имя',
            dataIndex: 'fname',
            flex:1
        }, {
            header: 'Отчество',
            dataIndex: 'patronymic',
            flex:1
        }, {
            header: 'Дата рождения',
            dataIndex: 'birthday',
            xtype:'datecolumn',
            format: 'd/m/Y',
            flex:1
        }];

        this.callParent(arguments);
    }
});
