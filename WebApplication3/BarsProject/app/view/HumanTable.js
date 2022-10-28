Ext.define('BarsProject.view.HumanTable' ,{
    extend: 'Ext.grid.Panel',
    alias: 'widget.humantable',
    store: 'BarsProject.store.HumanStore',
    dockedItems: [{
        xtype: 'pagingtoolbar',
        store: 'BarsProject.store.HumanStore',
        id: 'pagingbar',
        dock: 'bottom',
        prevText: 'Предыдущая',
        nextText: 'Следующая',
        emptyMsg: 'Нет данных',
        lastText: 'Последняя',
        firstText: 'Первая',
        refreshText: 'Обновить',
        beforePageText: 'Страница',
        displayMsg: 'Отображается {0} - {1} of {2}',
        displayInfo: true
    }],
    initComponent: function() {
        this.columns = [{
            header: 'Фамилия',
            sortable: false,
            hideable: false,
            dataIndex: 'surname',
            flex:1
        }, {
            header: 'Имя',
            dataIndex: 'fname',
            sortable: false,
            hideable: false,
            flex:1
        }, {
            header: 'Отчество',
            sortable: false,
            hideable: false,
            dataIndex: 'patronymic',
            flex:1
        }, {
            header: 'Дата рождения',
            dataIndex: 'birthday',
            hideable: false,
            sortable: false,
            xtype:'datecolumn',
            format: 'd.m.Y',
            flex:1
        }];

        this.callParent(arguments);
    }
});
