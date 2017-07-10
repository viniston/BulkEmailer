/**
 * @file 
 * Provides main Backbone view events and models.
 *
 * all te backbone events and models are presence here
 *
 * Author: Viniston Fernando
 */
$(document).ready(function () {

    $.fn.serializeObject = function () {
        var o = {};
        var a = this.serializeArray();
        $.each(a, function () {
            if (o[this.name]) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    };

    /**
    * Backbone view model.
    **/
    var ericaNominationModel = Backbone.Model.extend({
        defaults: {
            Quarter: moment().quarter(),
            Office: 1,
            AwardDate: 1,
            Subject: 1,
            SourceFile: 0
        }
    });

    /**
     * Backbone view.
     **/

    window.AppView = Backbone.View.extend({

        el: $(".bulkemailer"),

        // Main initialization entry point...

        initialize: function () {
            this.initInputForm();
        },

        initInputForm: function () {
            $('select').select2();
            $('.data-picker').datepicker({
                format: "mm-dd-yyyy",
                language: 'en',
                autoclose: true,
                defaultDate: moment().toDate()
            }).on('changeDate', function () {
                $('.data-picker').datepicker('hide');
            });
            $(':not([title=""])').qtip({
                style: {
                    classes: 'qtip-dark qtip-shadow qtip-rounded'
                }
            });
            $(".data-picker").datepicker('setDate', moment().toDate());
        },

        // Submit the Speed entry form with a help of WindModel...

        doUpload: function (e) {
            var template,
                $form,
                self = this;
            template = '{{#PropertyResult}}<tr><td><b>{{PropertyName}}</b></td><td>{{PropertyID}}</td><td>{{Rating}}</td><td>{{Country}}</td><td>{{Region}}</td><td>{{PropertyReferenceID}}</td>' +
                '<td>{{Resort}}</td><td><input type="button" class="btn btn-small btn-primary" value="select" /></td></tr>{{/PropertyResult}}';
            $form = this.$('#RegMetadata');

            // create model for controller
            var model = new FormData();
            model.append('Quarter', parseInt($.trim($form.find('[name="Quarter"]').val()), 9));
            model.append('Office', parseInt($.trim($form.find('[name="Office"]').val().toLowerCase()), 9));
            model.append('AwardDate', moment(new Date($form.find('[name="AwardDate"]').val())).format("YYYY-MM-DD"));
            model.append('Subject', $.trim($form.find('[name="Subject"]').val()));
            model.append('SourceFile', $form.find('[name="sourcefile"]')[0].files[0]);  // this has the file for sure

            $.ajax({
                url: "api/Common/UploadEricaTemplateWithData",
                type: 'POST',
                dataType: 'json',
                data: model,
                processData: false,
                contentType: false,// not json
                complete: function (data) {
                    var mediaId = $.parseJSON(data.responseText); //?

                },
                error: function (response) {
                    console.log(response.responseText);
                }
            });
            return true;

        },

        selectFile: function () {
            $("#img_text").html($('input[type="file"]').val());
        },

        // Backbone View events ...

        events: {
            "click #uploadbtn": "doUpload",
            "change input[type='file']": "selectFile"
        }

    });

    var appview = new AppView();
});