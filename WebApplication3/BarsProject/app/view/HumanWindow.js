Ext.define('BarsProject.view.HumanWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.humanwindow',

    title: 'Список анкет',
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
                width: 100,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    text: 'Выход',
                    handler: function (){
                        Ext.widget('humantable').getStore().data.removeAll();
                        Ext.widget('humantable').reconfigure(Ext.widget('humantable').getStore());
                        Ext.widget('humantable').store.currentPage = 1;
                        this.up('window').close();
                    },
                    margin: 5,
                    xtype: 'button'
                },{
                    text: 'Добавить',
                    action: 'add',
                    margin: 5,
                    xtype: 'button'
                },{
                    text: 'Изменить',
                    margin: 5,
                    action: 'edit',
                    xtype: 'button'
                },{
                    text: 'Удалить',
                    margin: 5,
                    action: 'delete',
                    xtype: 'button'
                },{
                    text: 'Печать',
                    margin: 5,
                    action: 'print',
                    xtype: 'button'
                }]
            })]
        })];
        this.callParent(arguments);
    }
});