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
            this.render();
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

        getMessage: function () {
            var self = this;

            $.ajax({
                url: "api/Common/GetEricaNominatorMessage/" + self.getParameterByName('nominationid'),
                type: 'Get',
                dataType: 'json',
                success: function (data) {
                    if (data.Response != null) {
                        if (data.Response.Message) {
                            self.$("#nominee-email-name").html(data.Response.NomineeName);
                            self.$("#replace-message-tag").replaceWith(data.Response.Message);
                            initSample();
                        }
                    }
                },
                error: function (response) {
                    $.notify("Error occured", "warn");
                }
            });
        },

        selectFile: function () {
            $("#img_text").html($('input[type="file"]').val());
        },

        processTemplate: function (e) {
            var editorElement = CKEDITOR.document.getById('editor');
            var message = editorElement.getHtml();
        },

        // Backbone View events ...

        events: {
            "click #uploadbtn": "doUpload",
            "change input[type='file']": "selectFile",
            "click .btn-process": "processTemplate"
        },

        render: function () {
            this.getMessage();
        }

    });

    var appview = new AppView();
});