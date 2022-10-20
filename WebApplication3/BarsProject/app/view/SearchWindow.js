Ext.define('BarsProject.view.SearchWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.searchwindow',

    title: 'Условия поиска',
    layout: 'fit',
    width: '85%',
    closable: false,
    modal: true,
    height: '50%',
    autoShow: true,

    initComponent: function() {
        this.items = [Ext.create('Ext.panel.Panel', {
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
                    name: 'surname'
                },{
                    xtype: 'textfield',
                    fieldLabel: 'Имя:',
                    name: 'fname'
                },{
                    xtype: 'textfield',
                    fieldLabel: 'Отчество:',
                    name: 'patronymic'
                },{
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    name: 'birthday'
                }]
            }), Ext.create('Ext.panel.Panel', {
                flex: 1,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    flex: 1,
                    text: 'Выход',
                    handler: function (){
                        this.up('window').close()
                    },
                    xtype: 'button'
                },{
                    flex: 1,
                    text: 'Начать поиск',
                    action: 'search',
                    xtype: 'button'
                }]
            })]
        })];
        this.callParent(arguments);
    }
});

