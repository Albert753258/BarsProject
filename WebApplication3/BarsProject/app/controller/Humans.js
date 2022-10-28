﻿Ext.define('BarsProject.controller.Humans', {
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
            },
            'humanwindow button[action=print]': {
                click: this.printTable
            }
        })
    },
    printTable: function(button){
        Ext.Date.patterns={
            MyFormat: "d.m.Y"
        };
        var store = Ext.widget('humantable').getStore();
        if(store.data.length != 0){
            var form = Ext.getCmp('searchform')
            window.open("/home/GenReport?surname=" + form.items.get(0).getValue() + "&fname=" + form.items.get(1).getValue() + "&birthdayFrom=" + Ext.Date.format(form.items.get(3).getValue(), Ext.Date.patterns.MyFormat) + "&birthdayTo=" + Ext.Date.format(form.items.get(4).getValue(), Ext.Date.patterns.MyFormat) + "&patronymic=" + form.items.get(2).getValue()
                , "_blank");
        }
        else {
            Ext.Msg.show({
                buttons: Ext.Msg.OK,
                title: 'Ошибка',
                msg: 'Нет данных для печати'
            });
        }
    },
    editHuman: function(button){
        //alert("startedit");
        var store = Ext.widget('humantable').getStore();
        var grid = Ext.widget('humantable');
        var window = button.up('window');
        var human = store.getAt(store.indexOf(window.sRec));
        var form = Ext.getCmp('hmform');
        //alert("New: " + form.items.get(0).getValue());
        if(form.items.get(0).getValue() != human.get('surname') || form.items.get(1).getValue() != human.get('fname') || form.items.get(2).getValue() != human.get('patronymic') || Ext.Date.format(form.items.get(3).getValue(), Ext.Date.patterns.MyFormat) != Ext.Date.format(human.get("birthday"), Ext.Date.patterns.MyFormat))
        {
            Ext.Msg.show({
                title:'Сохранить изменения?',
                closable: false,
                buttons: Ext.Msg.YESNOCANCEL,
                buttonText: {
                    yes: 'Да',
                    no: 'Нет',
                    cancel: 'Отмена'
                },
                fn: function(btn) {
                    if (btn === 'yes') {
                        if(form.isValid() && form.items.get(0).getValue() != '' && form.items.get(1).getValue() != '' && form.items.get(3).rawValue != ''){
                            Ext.Ajax.request({
                                url: '/home/EditHuman',
                                params: {
                                    "id": human.get("id"),
                                    "fname": form.items.get(1).getValue(),
                                    "surname": form.items.get(0).getValue(),
                                    "birthday": Ext.Date.format(form.items.get(3).getValue(), Ext.Date.patterns.MyFormat),
                                    "patronymic": form.items.get(2).getValue()
                                },
                                success: function (response){
                                    if(Ext.decode(response.responseText).error == 'duplicate'){
                                        Ext.Msg.show({
                                            title:'Изменить?',
                                            icon: Ext.Msg.QUESTION,
                                            msg: 'В базе уже есть этот человек',
                                            buttons: Ext.Msg.YESNO,
                                            buttonText: {
                                                yes: 'Да',
                                                no: 'Нет'
                                            },
                                            fn: function(btn) {
                                                if (btn === 'yes') {
                                                    Ext.Ajax.request({
                                                        url: '/home/EditHuman',
                                                        params: {
                                                            "id": human.get("id"),
                                                            "fname": form.items.get(1).getValue(),
                                                            "surname": form.items.get(0).getValue(),
                                                            "birthday": Ext.Date.format(form.items.get(3).getValue(), Ext.Date.patterns.MyFormat),
                                                            "patronymic": form.items.get(2).getValue(),
                                                            "confirm": 'yes'
                                                        },
                                                        success: function (response) {
                                                            store.each(function (record, idx){
                                                                if(record == human){
                                                                    record.set({
                                                                        id: human.get("id"),
                                                                        fname: form.items.get(1).getValue().toUpperCase(),
                                                                        surname: form.items.get(0).getValue().toUpperCase(),
                                                                        birthday: form.items.get(3).getValue(),
                                                                        patronymic: form.items.get(2).getValue().toUpperCase()
                                                                    });
                                                                    record.commit();
                                                                }
                                                            });
                                                            grid.reconfigure(store);
                                                            window.close();
                                                            close();
                                                        }
                                                    });
                                                } else if (btn === 'no') {
                                                    button.up('window').close();
                                                    close();
                                                }
                                            }
                                        });
                                    }
                                    else {
                                        store.each(function (record, idx){
                                            if(record == human){
                                                record.set({
                                                    id: human.get("id"),
                                                    fname: form.items.get(1).getValue().toUpperCase(),
                                                    surname: form.items.get(0).getValue().toUpperCase(),
                                                    birthday: form.items.get(3).getValue(),
                                                    patronymic: form.items.get(2).getValue().toUpperCase()
                                                });
                                                record.commit();
                                            }
                                        });
                                        grid.reconfigure(store);
                                        window.close();
                                        close();
                                    }
                                }
                            });
                        }
                        else {
                            Ext.Msg.show({
                                buttons: Ext.Msg.OK,
                                title: 'Вы ввели некорректные данные'
                            });
                        }
                    } else if (btn === 'no') {
                        window.close();
                        close();
                    } else {
                        close();
                    }
                }
            });
        }
        else {
            button.up('window').close();
        }
    },
    edit: function(button){
        var grid = button.up('window').down('grid');
        var selectedRecord = grid.getSelectionModel().getSelection()[0];
        if(grid.getSelectionModel().getCount() == 1){
            Ext.create('BarsProject.view.HumanForm', {action: 'edithuman', title: 'Редактирование анкеты', sRec: selectedRecord, cls: 'my-edit-window'});
            var form = Ext.getCmp('hmform');
            form.getForm().setValues(selectedRecord.getData());
        }
        else {
            Ext.Msg.show({
                buttons: Ext.Msg.OK,
                title: 'Ошибка',
                msg: 'Пожалуйста, выберите анкету для изменения'
            });
        }
    },
    search: function(){
        Ext.Date.patterns={
            MyFormat: "d.m.Y"
        };
        var form = Ext.getCmp('searchform');
        Ext.create('BarsProject.view.HumanWindow');
        var humantable = Ext.widget('humantable');
        humantable.store.currentPage = 1;
        humantable.store.proxy.extraParams = {
            start: 0,
            limit: 100,
            surname: form.items.get(0).getValue(),
            fname: form.items.get(1).getValue(),
            birthdayFrom: Ext.Date.format(form.items.get(3).getValue(), Ext.Date.patterns.MyFormat),
            birthdayTo: Ext.Date.format(form.items.get(4).getValue(), Ext.Date.patterns.MyFormat),
            patronymic: form.items.get(2).getValue()
        };
        humantable.store.load();
    },
    addNewHuman: function(button){
        var form = Ext.getCmp('hmform');
        if(form.items.get(1).getValue() == '' && form.items.get(0).getValue() == '' && form.items.get(2).getValue() == '' && form.items.get(3).rawValue == ''){
            button.up('window').close();
            close();
        }
        else{
            Ext.Msg.show({
                title:'Добавить?',
                msg: 'Вы уверены, что хотите добавить человека?',
                buttons: Ext.Msg.YESNO,
                icon: Ext.Msg.QUESTION,
                buttonText: {
                    yes: 'Да',
                    no: 'Нет'
                },
                fn: function(btn) {
                    if (btn === 'yes') {
                        Ext.Date.patterns={
                            MyFormat: "d.m.Y"
                        };
                        if(form.isValid() && form.items.get(0).getValue() != '' && form.items.get(1).getValue() != '' && form.items.get(3).rawValue != ''){
                            Ext.Ajax.request({
                                url: '/home/AddHuman',
                                params: {
                                    "fname": form.items.get(1).getValue(),
                                    "surname": form.items.get(0).getValue(),
                                    "birthday": Ext.Date.format(form.items.get(3).getValue(), Ext.Date.patterns.MyFormat),
                                    "patronymic": form.items.get(2).getValue()
                                },
                                success: function (response){
                                    if(Ext.decode(response.responseText).error == 'duplicate'){
                                        Ext.Msg.show({
                                            title:'Добавить?',
                                            icon: Ext.Msg.QUESTION,
                                            msg: 'В базе уже есть этот человек, уверенны что хотите добавить?',
                                            buttons: Ext.Msg.YESNO,
                                            buttonText: {
                                                yes: 'Да',
                                                no: 'Нет'
                                            },
                                            fn: function(btn) {
                                                if (btn === 'yes') {
                                                    Ext.Ajax.request({
                                                        url: '/home/AddHuman',
                                                        params: {
                                                            "fname": form.items.get(1).getValue(),
                                                            "surname": form.items.get(0).getValue(),
                                                            "birthday": Ext.Date.format(form.items.get(3).getValue(), Ext.Date.patterns.MyFormat),
                                                            "patronymic": form.items.get(2).getValue(),
                                                            "confirm": "yes"
                                                        },
                                                        success: function (response){
                                                            var store = Ext.widget('humantable').getStore();
                                                            store.add({
                                                                id: Ext.decode(response.responseText).id,
                                                                fname: form.items.get(1).getValue().toUpperCase(),
                                                                surname: form.items.get(0).getValue().toUpperCase(),
                                                                birthday: form.items.get(3).getValue(),
                                                                patronymic: form.items.get(2).getValue().toUpperCase()
                                                            });
                                                            close();
                                                            button.up('window').close();
                                                        }
                                                    });
                                                } else if (btn === 'no') {
                                                    button.up('window').close();
                                                    close();
                                                }
                                            }
                                        });
                                    }
                                    else {
                                        var store = Ext.widget('humantable').getStore();
                                        store.add({
                                            id: Ext.decode(response.responseText).id,
                                            fname: form.items.get(1).getValue().toUpperCase(),
                                            surname: form.items.get(0).getValue().toUpperCase(),
                                            birthday: form.items.get(3).getValue(),
                                            patronymic: form.items.get(2).getValue().toUpperCase()
                                        });
                                        button.up('window').close();
                                    }
                                }
                            });
                        }
                        else{
                            Ext.Msg.show({
                                buttons: Ext.Msg.OK,
                                title: 'Вы ввели некорректные данные'
                            });
                        }
                        
                    } else if (btn === 'no') {
                        button.up('window').close();
                        close();
                    }
                }
            });
        }
    },
    addHuman: function(button){
        Ext.create('BarsProject.view.HumanForm', {action: 'addnewhuman', title: 'Добавление анкеты', cls: 'my-add-window'});
    },
    deleteHuman: function(button){
        var grid = button.up('window').down('grid');
        var selectedRecord = grid.getSelectionModel().getSelection()[0];
        if(grid.getSelectionModel().getCount() == 1){
            Ext.Msg.show({
                title:'Удалить?',
                icon: Ext.Msg.QUESTION,
                msg: 'Вы уверены, что хотите удалить человека?',
                buttons: Ext.Msg.YESNO,
                buttonText: {
                    yes: 'Да',
                    no: 'Нет'
                },
                fn: function(btn) {
                    if (btn === 'yes') {
                        var store = Ext.widget('humantable').getStore();
                        Ext.Ajax.request({
                            url: '/home/DelHuman',
                            params: {
                                id: store.getAt(store.indexOf(selectedRecord)).get('id')
                            }
                        });
                        store.remove(selectedRecord);
                        grid.reconfigure(store);
                        Ext.getCmp('pagingbar').doRefresh();
                    } else if (btn === 'no') {
                        close();
                    }
                }
            });
        }
        else {
            Ext.Msg.show({
                buttons: Ext.Msg.OK,
                title: 'Ошибка',
                msg: 'Пожалуйста, выберите анкету для удаления'
            });
        }
    }
});