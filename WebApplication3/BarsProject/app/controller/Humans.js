Ext.define('BarsProject.controller.Humans', {
    extend: 'Ext.app.Controller',

    views: ['BarsProject.view.HumanTable', 'BarsProject.view.HumanWindow', 'BarsProject.view.SearchWindow', 'BarsProject.view.HumanForm'],
    stores: ['BarsProject.store.HumanStore'],
    models: ['BarsProject.model.Human'],
    init: function (){
        this.control({
            'searchwindow button[action=search]': {
                click: this.search
            },
            'humanwindow button[action=add]': {
                click: this.addHuman
            },
            'humanwindow button[action=edit]': {
                click: this.edit
            },
            'humanwindow button[action=delete]': {
                click: this.deleteHuman
            },
            'humanform button[action=addnewhuman]': {
                click: this.addNewHuman
            },
            'humanform button[action=edithuman]': {
                click: this.editHuman
            }
        })
    },
    editHuman: function(button){
        //alert("startedit");
        var store = Ext.widget('humantable').getStore();
        var window = button.up('window');
        var human = store.getAt(store.indexOf(window.sRec));
        //alert("Old: " + human.get('surname'));
        var form = Ext.getCmp('hmform');
        //alert("New: " + form.items.get(0).getValue());
        if(form.items.get(0).getValue() != human.get('surname') || form.items.get(1).getValue() != human.get('fname') || form.items.get(2).getValue() != human.get('patronymic') || form.items.get(3).getValue() != human.get('birthday'))
        {
            Ext.Msg.show({
                title:'Сохранить изменения?',
                closable: false,
                buttons: {xtype: button, },
                fn: function(btn) {
                    if (btn === 'yes') {
                        alert(human.get("id"));
                        Ext.Ajax.request({
                            url: '/home/EditHuman',
                            params: {
                                "id": human.get("id"),
                                "fname": form.items.get(1).getValue(),
                                "surname": form.items.get(0).getValue(),
                                "birthday": Ext.Date.format(form.items.get(3).getValue(), Ext.Date.patterns.MyFormat),
                                "patronymic": form.items.get(2).getValue()
                            }
                        });
                    } else if (btn === 'no') {
                        alert("Нет");
                        button.up('window').close();
                    } else {
                        close();
                    }
                }
            });
        }
        else {
            button.up('window').close();
        }
        //todo проверка наличия изменений
        //button.up('window').close();
    },
    edit: function(button){
        var grid = button.up('window').down('grid');
        var selectedRecord = grid.getSelectionModel().getSelection()[0];
        Ext.create('BarsProject.view.HumanForm', {action: 'edithuman', sRec: selectedRecord});
        var form = Ext.getCmp('hmform');
        form.getForm().setValues(selectedRecord.getData());
    },
    search: function(button){
        Ext.Date.patterns={
            MyFormat: "d.m.Y"
        };
        var form = Ext.getCmp('searchform');
        Ext.create('BarsProject.view.HumanWindow');
        var humantable = Ext.widget('humantable');
        humantable.store.proxy.extraParams = {
            surname: form.items.get(0).getValue(),
            fname: form.items.get(1).getValue(),
            birthday: Ext.Date.format(form.items.get(3).getValue(), Ext.Date.patterns.MyFormat),
            patronymic: form.items.get(2).getValue()
        };
        humantable.store.load();
    },
    addNewHuman: function(button){
        Ext.Date.patterns={
            MyFormat: "d.m.Y"
        };
        //var form = button.up('window').down('form');
        var form = Ext.getCmp('hmform');
        var store = Ext.widget('humantable').getStore();
        Ext.Ajax.request({
            url: '/home/AddHuman',
            params: {
                "fname": form.items.get(1).getValue(),
                "surname": form.items.get(0).getValue(),
                "birthday": Ext.Date.format(form.items.get(3).getValue(), Ext.Date.patterns.MyFormat),
                "patronymic": form.items.get(2).getValue()
            }
        });
        button.up('window').close();
    },
    addHuman: function(button){
        Ext.create('BarsProject.view.HumanForm', {action: 'addnewhuman'});
    },
    deleteHuman: function(button){
        var grid = button.up('window').down('grid');
        var selectedRecord = grid.getSelectionModel().getSelection()[0];
        var store = Ext.widget('humantable').getStore();
        Ext.Ajax.request({
            url: '/home/DelHuman',
            params: {
                id: store.getAt(store.indexOf(selectedRecord)).get('id')
            }
        });
        //alert(store.getAt(store.indexOf(selectedRecord)).get('id'))
        store.remove(selectedRecord);
        grid.getView().refresh();
    }
});