Ext.define('BarsProject.view.HumanForm', {
    extend: 'Ext.window.Window',
    alias: 'widget.humanform',
    
    title: 'Анкета гражданина',
    layout: 'fit',
    width: '85%',
    modal: true,
    action: "addnewhuman",
    height: '50%',
    autoShow: true,

    initComponent: function() {
        this.items = [Ext.create('Ext.panel.Panel', {
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            items: [Ext.create('Ext.panel.Panel', {
                flex: 10,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [Ext.create('Ext.form.Panel',{
                    id: 'hmform',
                    items:[{
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
                })]
            }), {
                flex: 1,
                text: 'Выход',
                action: this.action,
                xtype: 'button'
            }]
        })];
        this.callParent(arguments);
    }
});


