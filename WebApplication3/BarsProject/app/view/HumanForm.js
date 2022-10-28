Ext.define('BarsProject.view.HumanForm', {
    extend: 'Ext.window.Window',
    alias: 'widget.humanform',
    layout: 'fit',
    width: '85%',
    closable: false,
    modal: true,
    action: "addnewhuman",
    height: '50%',
    cls: 'my-edit-window',
    title: 'Анкета человека',
    autoShow: true,

    initComponent: function() {
        this.items = [Ext.create('Ext.panel.Panel', {
            layout: {
                type: 'hbox',
                align: 'stretch'
            },
            items: [Ext.create('Ext.panel.Panel', {
                flex: 10,
                frame: false,
                bodyStyle: 'background-color: transparent;',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [Ext.create('Ext.form.Panel',{
                    id: 'hmform',
                    items:[{
                        xtype: 'textfield',
                        fieldLabel: 'Фамилия:',
                        margin: 5,
                        width: 500,
                        vtype: 'surname',
                        name: 'surname'
                    },{
                        xtype: 'textfield',
                        fieldLabel: 'Имя:',
                        vtype: 'fname',
                        width: 500,
                        margin: 5,
                        name: 'fname'
                    },{
                        xtype: 'textfield',
                        fieldLabel: 'Отчество:',
                        vtype: 'patronymic',
                        width: 500,
                        margin: 5,
                        name: 'patronymic'
                    },{
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        width: 500,
                        margin: 5,
                        minText: 'Дата в данном поле не должна быть меньше {0}',
                        maxText: 'Дата в данном поле не должна быть больше {0}',
                        startDay: 1,
                        fieldLabel: 'Дата рождения:',
                        minValue: new Date(1900, 0, 0),
                        maxValue: new Date(),
                        name: 'birthday'
                    }]
                })]
            }),Ext.create('Ext.panel.Panel', {
                flex: 1,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [{
                    text: 'Выход',
                    margin: 5,
                    action: this.action,
                    xtype: 'button'
                }]
            })]
        })];
        this.callParent(arguments);
    }
});


