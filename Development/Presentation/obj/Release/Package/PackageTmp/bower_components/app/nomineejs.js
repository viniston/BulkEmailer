/**
 * @file 
 * Provides main Backbone view events and models.
 *
 * all te backbone events and models are presence here
 *
 * Author: Viniston Fernando
 */
$(document).ready(function () {


    /**
     * Backbone view.
     **/

    window.AppView = Backbone.View.extend({

        el: $(".bulkemailer"),

        // Main initialization entry point...

        initialize: function () {
            this
              .historyTemplate =
              '{{#Response}}<tr><td><b>{{NomineeName}}</b></td><td>{{NomineeEmail}} <i title="{{NominatorEmailList}}" class="icon-info-sign signInfo"></td><td>{{Status}}</td><td><input type="button" id="{{Id}}" class="btn btn-small btn-primary btn-process" value="Send Mail" /></td></tr>{{/Response}}';
            this.render();
        },


        // Submit the Speed entry form with a help of WindModel...

        doUpload: function (e) {
            var $form,
                self = this;
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
                contentType: false,
                success: function (data) {
                    $.notify("Erica nomination list uploaded successfully", "success");
                    if (data.Response != null) {
                        var htm = Mustache.render(self.historyTemplate, data);
                        self.$("#tbdyHistory").html(htm);
                    }
                },
                error: function (response) {
                    $.notify("Erica nomination list not uploaded", "warn");
                }
            });
            return true;

        },

        getParameterByName: function (name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, "\\$&");
            var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, " "));
        },

        getHistory: function () {
            var self = this;

            $.ajax({
                url: "api/Common/GetEricaNomineeList/" + self.getParameterByName('ericaid'),
                type: 'Get',
                dataType: 'json',
                success: function (data) {
                    if (data.Response != null) {
                        var htm = Mustache.render(self.historyTemplate, data);
                        self.$("#tbdyHistory-nominees").html(htm);
                        $(':not([title=""])').qtip({
                            style: {
                                classes: 'qtip-dark qtip-shadow qtip-rounded'
                            }
                        });
                    }
                },
                error: function (response) {
                    $.notify("Erica nomination list not uploaded", "warn");
                }
            });
        },

        selectFile: function () {
            $("#img_text").html($('input[type="file"]').val());
        },

        processTemplate: function (e) {
            window.location = "EricaSendMail.html?nominationid=" + e.target.id + "";
        },

        // Backbone View events ...

        events: {
            "click #uploadbtn": "doUpload",
            "change input[type='file']": "selectFile",
            "click .btn-process": "processTemplate"
        },

        render: function () {
            this.getHistory();
        }

    });

    var appview = new AppView();
});