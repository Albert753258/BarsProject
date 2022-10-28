Ext.Loader.setConfig({
    enabled: true,
    disableCaching: true,
    paths: {
        Ext: '.',
        'Ext.ux': 'BarsProject/ux'
    }
});
Ext.require([
    'Ext.ux.grid.Printer',
    'Ext.ux.RowExpander'
]);
Ext.application({
    name: 'BarsProject',
    appFolder: 'BarsProject/app',
    controllers: ['Humans'],
    //requires: ['BarsProject.view.HumanTable'],
    launch: function() {
        Ext.Date.defaultFormat='d.m.Y';
        var fnameVType = {
            fname: function (val, field){
                return /(^[а-яёА-ЯЁ]{1,40}(?:-[а-яёА-ЯЁ]{1,40})*$)|(^[а-яёА-ЯЁ]{1,40}(?: [а-яёА-ЯЁ]{1,40})*$)/.test(val) && val.length <= 40;
            },
            fnameText: 'Имя введено неверно'
        }
        Ext.apply(Ext.form.field.VTypes, fnameVType);
        var surnameVType = {
            surname: function (val, field){
                return /^[а-яёА-ЯЁ]{2,40}(?:-[а-яёА-ЯЁ]{1,40})?$/.test(val) && val.length <= 40;
            },
            surnameText: 'Фамилия введена неверно'
        }
        Ext.apply(Ext.form.field.VTypes, surnameVType);
        var patronymicVType = {
            patronymic: function (val, field){
                return /^[а-яёА-ЯЁ]*$/.test(val) && val.length <= 40;
            },
            patronymicText: 'Отчество введено неверно'
        }
        Ext.apply(Ext.form.field.VTypes, patronymicVType);
        Ext.Date.dayNames = ['Воскресенье', 'Понедельник', 'Вторник', 'Среда', 'Четверг', 'Пятница', 'Суббота'];
        Ext.Date.monthNames = ['Январь','Февраль','Март', 'Апрель', 'Май', 'Июнь', 'Июль', 'Август', 'Сентябрь', 'Октябрь', 'Ноябрь', 'Декабрь'];
        var alertButton = Ext.create('Ext.Button', {
            margin:'10 0 0 30',
            renderTo: Ext.getBody(),
            text: 'Начать работу',
            handler: function(){
                Ext.create('BarsProject.view.SearchWindow');
            },
            scope:this
        });
        // Ext.Ajax.request({
        //     url: '/home/SearchHuman',
        //     params: {surname: 'Nasyrov'},
        //     success: function(response){
        //         alert(response.responseText);
        //     }
        // });

    }
});