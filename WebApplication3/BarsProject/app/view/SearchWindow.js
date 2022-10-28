Ext.define('BarsProject.view.SearchWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.searchwindow',
    title: 'Условия поиска',
    layout: 'fit',
    width: '90%',
    style: {'background-color': 'red'},
    closable: false,
    modal: true,
    height: '60%',
    autoShow: true,

    initComponent: function() {
        this.items = [Ext.create('Ext.panel.Panel', {
            frame: false,
            bodyStyle: 'background-color: transparent;',
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            items: [Ext.create('Ext.form.Panel',{
                id: 'searchform',
                flex: 10,
                items: [{
                    xtype: 'textfield',
                    fieldLabel: 'Фамилия:',
                    width: 500,
                    margin: 5,
                    name: 'surname'
                },{
                    xtype: 'textfield',
                    fieldLabel: 'Имя:',
                    width: 500,
                    margin: 5,
                    name: 'fname'
                },{
                    xtype: 'textfield',
                    fieldLabel: 'Отчество:',
                    width: 500,
                    margin: 5,
                    name: 'patronymic'
                },{
                    xtype: 'datefield',
                    fieldLabel: 'Дата рождения с:',
                    format: 'd.m.Y',
                    width: 500,
                    margin: 5,
                    startDay: 1,
                    name: 'birthday'
                },{
                    xtype: 'datefield',
                    fieldLabel: 'Дата рождения по:',
                    format: 'd.m.Y',
                    width: 500,
                    margin: 5,
                    startDay: 1,
                    name: 'birthday'
                }]
            }), Ext.create('Ext.panel.Panel', {
                width: 150,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    text: 'Выход',
                    handler: function (){
                        this.up('window').close()
                    },
                    margin: 5,
                    xtype: 'button'
                },{
                    text: 'Начать поиск',
                    action: 'search',
                    margin: 5,
                    xtype: 'button'
                }]
            })]
        })];
        this.callParent(arguments);
    }
});

