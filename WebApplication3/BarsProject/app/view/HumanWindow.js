Ext.define('BarsProject.view.HumanWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.humanwindow',

    title: 'Результаты поиска',
    layout: 'fit',
    width: '85%',
    modal: true,
    height: '85%',
    closable: false,
    autoShow: true,
    
    initComponent: function() {
        //this.items = [{xtype: 'humantable'}];
        this.items = [Ext.create('Ext.panel.Panel', {
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            items: [{
                xtype: 'humantable',
                flex: 10
            }, Ext.create('Ext.panel.Panel', {
                flex: 1,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    scale: 'large',
                    text: 'Выход',
                    handler: function (){
                        this.up('window').close()
                    },
                    xtype: 'button'
                },{
                    scale: 'large',
                    text: 'Добавить',
                    action: 'add',
                    xtype: 'button'
                },{
                    scale: 'large',
                    text: 'Изменить',
                    action: 'edit',
                    xtype: 'button'
                },{
                    scale: 'large',
                    text: 'Удалить',
                    action: 'delete',
                    xtype: 'button'
                },{
                    scale: 'large',
                    text: 'Печать',
                    action: 'print',
                    xtype: 'button'
                }]
            })]
        })];
        this.callParent(arguments);
    }
});